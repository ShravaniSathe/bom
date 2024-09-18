using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using bom.Models.BOMStructures;
using bom.Repositories.BOMStructures.Abstractions;

namespace bom.Repositories.BOMStructures.Implementations
{
    public class BOMStructuresRepository : Repository<BOMStructure>, IBOMStructuresRepository
    {
        public BOMStructuresRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<BOMStructure> AddBOMStructureAsync(BOMStructure bomStructure)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.BOMStructures (ItemMasterSalesId, ParentRawMaterialId, ChildRawMaterialId) " +
                                       "VALUES (@ItemMasterSalesId, @ParentRawMaterialId, @ChildRawMaterialId); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        bomStructure.ItemMasterSalesId,
                        bomStructure.ParentRawMaterialId,
                        bomStructure.ChildRawMaterialId
                    }, transaction: tran);

                    bomStructure.Id = id.Single();

                    CommitTransaction(tran);

                    return bomStructure;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<BOMStructure> GetBOMStructureAsync(int id)
        {
            const string storedProcedureName = "dbo.GetBOMStructureById";

            var result = await db.QueryAsync<BOMStructure>(storedProcedureName,
                                                            new { Id = id },
                                                            commandType: CommandType.StoredProcedure
                                                            ).ConfigureAwait(false);

            return result.SingleOrDefault();
        }

        public async Task<IEnumerable<BOMStructure>> GetAllBOMStructuresAsync()
        {
            const string storedProcedureName = "dbo.GetAllBOMStructures";

            var result = await db.QueryAsync<BOMStructure>(storedProcedureName,
                                                            commandType: CommandType.StoredProcedure
                                                            ).ConfigureAwait(false);

            return result;
        }

        public async Task<bool> UpdateBOMStructureAsync(BOMStructure bomStructure)
        {
            const string storedProcedureName = "dbo.UpdateBOMStructure";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   bomStructure.Id,
                                                   bomStructure.ItemMasterSalesId,
                                                   bomStructure.ParentRawMaterialId,
                                                   bomStructure.ChildRawMaterialId
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task DeleteBOMStructureAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteBOMStructureById";

            await db.ExecuteAsync(storedProcedureName,
                                  new { Id = id },
                                  commandType: CommandType.StoredProcedure
                                  ).ConfigureAwait(false);
        }
    }
}
