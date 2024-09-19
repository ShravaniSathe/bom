using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using bom.Models.BOMStructure;
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
                    // Insert BOMTree record
                    const string insertBOMTreeSql = "INSERT INTO dbo.BOMTrees (BOMName) " +
                                                    "VALUES (@BOMName); " +
                                                    "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var bomId = await db.QuerySingleAsync<int>(insertBOMTreeSql,
                        new { bomTree.BOMName }, transaction: tran);

                    bomTree.BOMId = bomId;

                    // Insert BOMTreeNode records
                    foreach (var node in bomTree.Nodes)
                    {
                        await InsertBOMTreeNodeAsync(bomTree.BOMId, node, tran);
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
            const string insertBOMTreeNodeSql = "INSERT INTO dbo.BOMTreeNodes (BOMId, Name, ParentId) " +
                                                "VALUES (@BOMId, @Name, @ParentId); " +
                                                "SELECT CAST(SCOPE_IDENTITY() as int)";
            var nodeId = await db.QuerySingleAsync<int>(insertBOMTreeNodeSql, new
            {
                BOMId = bomId,
                node.Name,
                ParentId = node.ParentId.HasValue ? (object)node.ParentId.Value : DBNull.Value
            }, transaction: transaction);

            node.Id = nodeId;

            foreach (var child in node.Children)
            {
                await InsertBOMTreeNodeAsync(bomId, child, transaction);
            }
        }

        public async Task<BOMTree> GetBOMTreeByIdAsync(int bomId)
        {
            const string storedProcedureName = "sp_GetBOMTreeById";

            var result = await db.QueryMultipleAsync(storedProcedureName,
                new { BOMId = bomId }, commandType: CommandType.StoredProcedure);

            var bomTree = result.Read<BOMTree>().SingleOrDefault();
            var nodes = result.Read<BOMTreeNode>().ToList();

            if (bomTree != null)
            {
                // Create a dictionary to easily access nodes by their Id
                var nodeDictionary = nodes.ToDictionary(n => n.Id, n => n);

                // Initialize the Children property for all nodes
                foreach (var node in nodes)
                {
                    node.Children = new List<BOMTreeNode>();
                }

                // Assign children to their parents
                foreach (var node in nodes)
                {
                    if (node.ParentId.HasValue && nodeDictionary.TryGetValue(node.ParentId.Value, out var parentNode))
                    {
                        parentNode.Children.Add(node);
                    }
                }

                // Set the root nodes (nodes without a parent)
                bomTree.Nodes = nodes.Where(n => !n.ParentId.HasValue).ToList();
            }

            return bomTree;
        }

        public async Task<BOMTree> UpdateBOMTreeAsync(BOMTree bomTree)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    // Update BOMTree record
                    const string updateBOMTreeSql = "UPDATE dbo.BOMTrees SET BOMName = @NewBOMName WHERE BOMId = @Id";
                    await db.ExecuteAsync(updateBOMTreeSql,
                        new { Id = bomTree.BOMId, NewBOMName = bomTree.BOMName },
                        transaction: tran);

                    // Update BOMTreeNode records
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
            const string updateBOMTreeNodeSql = "UPDATE dbo.BOMTreeNodes SET Name = @NewName, ParentId = @NewParentId " +
                                                 "WHERE Id = @Id";
            await db.ExecuteAsync(updateBOMTreeNodeSql, new
            {
                Id = node.Id,
                NewName = node.Name,
                NewParentId = node.ParentId.HasValue ? (object)node.ParentId.Value : DBNull.Value
            }, transaction: transaction);

            foreach (var child in node.Children)
            {
                await UpdateBOMTreeNodeAsync(child, transaction);
            }
        }

        public async Task DeleteBOMTreeAsync(int bomId)
        {
            const string deleteBOMTreeNodesSql = "DELETE FROM dbo.BOMTreeNodes WHERE BOMId = @BOMId";
            const string deleteBOMTreeSql = "DELETE FROM dbo.BOMTrees WHERE BOMId = @BOMId";

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
