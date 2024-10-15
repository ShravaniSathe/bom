using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using bom.Models.BOMStructure;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;
using bom.Models.SubAssemblies;
using bom.Repositories.BOMStructures.Abstractions;

namespace bom.Repositories.BOMStructures.Implementations
{
    public class BOMTreeRepository : Repository<BOMTree>, IBOMTreeRepository
    {
        public BOMTreeRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<BOMTree> CreateBOMTreeAsync(BOMTree bomTree)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    // Insert BOMTree and get its Id
                    const string insertBOMTreeSql = "INSERT INTO dbo.BOMTrees (ItemMasterSalesId) " +
                                                    "VALUES (@ItemMasterSalesId); " +
                                                    "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var bomId = await db.QuerySingleAsync<int>(insertBOMTreeSql,
                        new { bomTree.ItemMasterSalesId }, transaction: tran);

                    bomTree.Id = bomId;

                    // Insert top-level nodes (like Seats and Engine)
                    foreach (var node in bomTree.Nodes)
                    {
                        // For the top-level nodes, set ParentId to the ItemMasterSalesId
                        node.ParentId = bomTree.ItemMasterSalesId;
                        node.Level = 1; // Root nodes are at level 1

                        // Insert the top-level node and its children
                        await InsertBOMTreeNodeAsync(bomTree.Id, node, tran);
                    }

                    CommitTransaction(tran);
                    return bomTree;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        private async Task InsertBOMTreeNodeAsync(int bomId, BOMTreeNode node, IDbTransaction transaction)
        {
            // If ParentId is 0 or not set, use DBNull.Value or null
            var parentIdValue = node.ParentId != 0 ? (object)node.ParentId : DBNull.Value;

            const string insertBOMTreeNodeSql = @"
        INSERT INTO dbo.BOMTreeNodes (BOMId, Name, ParentId, Level, NodeType, PType) 
        VALUES (@BOMId, @Name, @ParentId, @Level, @NodeType, @PType); 
        SELECT CAST(SCOPE_IDENTITY() as int)";

            // Insert the current node and get its Id
            var nodeId = await db.QuerySingleAsync<int>(insertBOMTreeNodeSql, new
            {
                BOMId = bomId,
                Name = node.Name,
                ParentId = parentIdValue,  // If ParentId is 0, it will be DBNull.Value (for top-level nodes)
                Level = node.Level,
                NodeType = node.NodeType,
                PType = node.PType
            }, transaction: transaction);

            // Set the newly inserted node's Id
            node.Id = nodeId;

            // Insert child nodes, if any
            foreach (var child in node.Children)
            {
                // Set the child's ParentId to the newly inserted node's Id
                child.ParentId = node.Id;
                child.Level = node.Level + 1; // Increment the level for the child

                // Recursively insert child nodes
                await InsertBOMTreeNodeAsync(bomId, child, transaction);
            }
        }


        public async Task<BOMTree> GetBOMTreeByIdAsync(int bomId)
        {
            const string storedProcedureName = "sp_GetBOMTreeById";

            // Execute the stored procedure to get multiple result sets
            var result = await db.QueryMultipleAsync(storedProcedureName,
                new { Id = bomId }, commandType: CommandType.StoredProcedure);

            // 1. Read and map the BOMTree object
            var bomTree = result.Read<BOMTree>().SingleOrDefault();
            var itemMasterSales = result.Read<ItemMasterSale>().SingleOrDefault();
            var nodes = result.Read<BOMTreeNode>().ToList();
            var subAssemblies = result.Read<SubAssemblie>().ToList();
            var rawMaterials = result.Read<ItemMasterRawMaterial>().ToList();

            if (bomTree != null)
            {
                // Attach ItemMasterSales to the BOMTree object
                bomTree.ItemMasterSales = itemMasterSales;

                // 2. Create a dictionary to map nodes by their ID (to facilitate parent-child relationships)
                var nodeDictionary = nodes.ToDictionary(n => n.Id);

                // 3. Initialize empty lists for node children and sub-assemblies
                foreach (var node in nodes)
                {
                    node.Children = new List<BOMTreeNode>();
                    node.SubAssemblies = new List<SubAssemblie>();
                }

                // 4. Establish parent-child relationships among nodes
                foreach (var node in nodes)
                {
                    if (node.ParentId.HasValue && nodeDictionary.TryGetValue(node.ParentId.Value, out var parentNode))
                    {
                        parentNode.Children.Add(node);  // Add the node as a child of its parent
                    }
                }

                // 5. Get top-level nodes (without ParentId) and assign to BOMTree
                bomTree.Nodes = nodes.Where(n => !n.ParentId.HasValue).ToList();

                // 6. Attach sub-assemblies to their respective nodes
                foreach (var subAssembly in subAssemblies)
                {
                    // Find the parent node (based on ItemMasterSalesId) and add the sub-assembly
                    if (nodeDictionary.TryGetValue((int)subAssembly.ParentSubAssemblyId, out var parentNode))
                    {
                        parentNode.SubAssemblies.Add(subAssembly);
                    }
                }

                // 7. Attach raw materials to their respective sub-assemblies
                foreach (var rawMaterial in rawMaterials)
                {
                    var parentSubAssembly = subAssemblies.FirstOrDefault(sa => sa.Id == rawMaterial.SubAssemblyId);
                    if (parentSubAssembly != null)
                    {
                        parentSubAssembly.RawMaterials ??= new List<ItemMasterRawMaterial>();
                        parentSubAssembly.RawMaterials.Add(rawMaterial);
                    }
                }
            }

            return bomTree;
        }

        public async Task<BOMTree> UpdateBOMTreeAsync(BOMTree bomTree)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string updateBOMTreeSql = "UPDATE dbo.BOMTrees SET ItemMasterSalesId = @NewItemMasterSalesId WHERE Id = @Id";
                    await db.ExecuteAsync(updateBOMTreeSql,
                        new { Id = bomTree.Id, NewItemMasterSalesId = bomTree.ItemMasterSalesId },
                        transaction: tran);

                    foreach (var node in bomTree.Nodes)
                    {
                        await UpdateBOMTreeNodeAsync(node, tran);
                    }

                    CommitTransaction(tran);
                    return bomTree;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        private async Task UpdateBOMTreeNodeAsync(BOMTreeNode node, IDbTransaction transaction)
        {
            const string updateBOMTreeNodeSql = "UPDATE dbo.BOMTreeNodes SET Name = @NewName, ParentId = @NewParentId, NodeType = @NodeType, PType = @PType " +
                                                 "WHERE Id = @Id";
            await db.ExecuteAsync(updateBOMTreeNodeSql, new
            {
                Id = node.Id,
                NewName = node.Name,
                NewParentId = node.ParentId.HasValue ? (object)node.ParentId.Value : DBNull.Value,
                NodeType = node.NodeType,
                PType = node.PType
            }, transaction: transaction);

            foreach (var child in node.Children)
            {
                await UpdateBOMTreeNodeAsync(child, transaction);
            }
        }

        public async Task DeleteBOMTreeAsync(int bomId)
        {
            const string deleteBOMTreeNodesSql = "DELETE FROM dbo.BOMTreeNodes WHERE BOMId = @BOMId";
            const string deleteBOMTreeSql = "DELETE FROM dbo.BOMTrees WHERE Id = @BOMId"; // Use Id instead of BOMId

            using (var tran = BeginTransaction())
            {
                try
                {
                    await db.ExecuteAsync(deleteBOMTreeNodesSql, new { BOMId = bomId }, transaction: tran);
                    await db.ExecuteAsync(deleteBOMTreeSql, new { BOMId = bomId }, transaction: tran);

                    CommitTransaction(tran);
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }
    }
}
