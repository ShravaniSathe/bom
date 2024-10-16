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
using System.Data.SqlClient;

namespace bom.Repositories.BOMStructures.Implementations
{
    public class BOMTreeRepository : Repository<BOMTree>, IBOMTreeRepository
    {
        private readonly IDbConnection _dbConnection;

        public BOMTreeRepository(string connectionString) : base(connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
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


        public async Task<BOMTree> GetBOMTreeByIdAsync(int bomTreeId)
        {
            const string bomTreeSql = @"
             SELECT 
             bt.Id,
             bt.ItemMasterSalesId,
             ims.ID, ims.ItemName, ims.ItemCode, ims.Grade, ims.UOM, ims.Quantity, ims.Level, ims.PType, ims.CreatedDate
             FROM 
             dbo.BOMTrees bt
             INNER JOIN 
             dbo.ItemMasterSales ims ON bt.ItemMasterSalesId = ims.ID
             WHERE 
             bt.Id = @BOMTreeId;
            ";

            const string subAssembliesSql = @"
             SELECT 
             sa.ID,
             sa.ItemName,
             sa.UOM,
             sa.CostPerUnit,
             sa.ItemMasterSalesId,
             sa.Level,
             sa.ParentSubAssemblyId,
             sa.PType,
             sa.Quantity
             FROM 
             dbo.SubAssemblies sa
             WHERE 
             sa.ItemMasterSalesId = @ItemMasterSalesId;
            ";

            const string rawMaterialsSql = @"
             SELECT 
             rm.ID,
             rm.SubAssemblyId,
             rm.ItemName,
             rm.ItemCode,
             rm.Grade,
             rm.UOM,
             rm.Quantity,
             rm.Level,
             rm.PType,
             rm.CostPerUnit
             FROM 
             dbo.ItemMasterRawMaterials rm
             WHERE 
             rm.SubAssemblyId IN (SELECT ID FROM dbo.SubAssemblies WHERE ItemMasterSalesId = @ItemMasterSalesId);
            ";

            const string bomTreeNodesSql = @"
             SELECT 
             btn.Id,
             btn.Name,
             btn.ParentId,
             btn.Level,
             btn.NodeType,
             btn.PType,
             btn.BOMId
             FROM 
             dbo.BOMTreeNodes btn
             WHERE 
             btn.BOMId = @BOMTreeId;
            ";

            // Step 1: Retrieve BOMTree and ItemMasterSales
            var bomTreeRecord = await _dbConnection.QueryAsync<BOMTree, ItemMasterSale, BOMTree>(bomTreeSql,
                (bomTree, itemMasterSale) =>
                {
                    bomTree.ItemMasterSales = itemMasterSale;
                    return bomTree;
                },
                new { BOMTreeId = bomTreeId });

            var bomTree = bomTreeRecord.FirstOrDefault();

            if (bomTree == null)
            {
                return null; // No BOM tree found
            }

            // Step 2: Retrieve SubAssemblies
            var subAssemblies = await _dbConnection.QueryAsync<SubAssemblie>(subAssembliesSql,
                new { ItemMasterSalesId = bomTree.ItemMasterSalesId });

            // Step 3: Retrieve RawMaterials for SubAssemblies
            var rawMaterials = await _dbConnection.QueryAsync<ItemMasterRawMaterial>(rawMaterialsSql,
                new { ItemMasterSalesId = bomTree.ItemMasterSalesId });

            // Step 4: Retrieve BOMTreeNodes
            var bomTreeNodes = await _dbConnection.QueryAsync<BOMTreeNode>(bomTreeNodesSql,
                new { BOMTreeId = bomTree.Id });

            // Step 5: Create dictionaries for fast lookup
            var nodeDictionary = bomTreeNodes.ToDictionary(n => n.Id, n => n);
            var subAssemblyDictionary = subAssemblies.ToDictionary(sa => sa.Id, sa => sa);

            // Step 6: Build the hierarchy for SubAssemblies
            foreach (var subAssembly in subAssemblies)
            {
                if (subAssembly.ParentSubAssemblyId == null)
                {
                    // This subassembly is a direct child of ItemMasterSales (top-level node)
                    bomTree.SubAssemblies.Add(subAssembly);
                }
                else if (subAssemblyDictionary.ContainsKey(subAssembly.ParentSubAssemblyId.Value))
                {
                    // This subassembly is a child of another subassembly
                    var parentSubAssembly = subAssemblyDictionary[subAssembly.ParentSubAssemblyId.Value];
                    parentSubAssembly.ChildSubAssemblies.Add(subAssembly);
                }
            }

            // Step 7: Build the hierarchy for RawMaterials (updated logic)
            foreach (var rawMaterial in rawMaterials)
            {
                if (subAssemblyDictionary.ContainsKey(rawMaterial.SubAssemblyId))
                {
                    // If the raw material belongs to a sub-assembly, add it to that sub-assembly
                    var subAssembly = subAssemblyDictionary[rawMaterial.SubAssemblyId];
                    subAssembly.RawMaterials ??= new List<ItemMasterRawMaterial>();
                    subAssembly.RawMaterials.Add(rawMaterial);
                }
                else
                {
                    // If it doesn't belong to any sub-assembly, add it as a top-level raw material in ItemMasterSales
                    bomTree.RawMaterials.Add(rawMaterial);
                }
            }


            // Step 8: Build BOM Tree Nodes hierarchy
            foreach (var node in bomTreeNodes)
            {
                if (node.ParentId == bomTree.ItemMasterSalesId)
                {
                    // This node is a direct child of ItemMasterSales (top-level node)
                    bomTree.Nodes.Add(node);
                }
                else if (nodeDictionary.ContainsKey((int)node.ParentId))
                {
                    // This node is a child of another node
                    nodeDictionary[(int)node.ParentId].Children.Add(node);
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
