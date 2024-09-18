using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bom.Models.BOMStructure;
using bom.Repositories.BOMStructures.Abstractions;
using Dapper;

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
                    var parameters = new DynamicParameters();
                    parameters.Add("BOMName", bomTree.BOMName);
                    parameters.Add("BOMId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await db.ExecuteAsync("sp_InsertBOMTree", parameters, commandType: CommandType.StoredProcedure, transaction: tran);

                    bomTree.BOMId = parameters.Get<int>("BOMId");

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
            var parameters = new DynamicParameters();
            parameters.Add("BOMId", bomId);
            parameters.Add("Name", node.Name);
            parameters.Add("ParentId", node.ParentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("NodeId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await db.ExecuteAsync("sp_InsertBOMTreeNode", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

            var nodeId = parameters.Get<int>("NodeId");

            foreach (var child in node.Children)
            {
                await InsertBOMTreeNodeAsync(bomId, child, transaction);
            }
        }

        public async Task<BOMTree> GetBOMTreeByIdAsync(int bomId)
        {
            const string storedProcedureName = "sp_GetBOMTreeById";

            var result = await db.QueryMultipleAsync(storedProcedureName, new { BOMId = bomId }, commandType: CommandType.StoredProcedure);

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



        public async Task UpdateBOMTreeAsync(BOMTree bomTree)
        {
            const string storedProcedureName = "sp_UpdateBOMTree";

            await db.ExecuteAsync(storedProcedureName, new
            {
                BOMId = bomTree.BOMId,
                BOMName = bomTree.BOMName
            }, commandType: CommandType.StoredProcedure);

            // Update BOM Tree Nodes
            // (Additional implementation if needed)
        }

        public async Task DeleteBOMTreeAsync(int bomId)
        {
            const string storedProcedureName = "sp_DeleteBOMTree";

            await db.ExecuteAsync(storedProcedureName, new { BOMId = bomId }, commandType: CommandType.StoredProcedure);
        }
    }
}
