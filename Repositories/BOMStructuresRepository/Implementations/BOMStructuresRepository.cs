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
                    const string sql = "INSERT INTO dbo.BOMStructures (ItemMasterSalesId, ParentSubAssemblyId, ChildSubAssemblyId, ChildRawMaterialId, Level, PType) " +
                    "VALUES (@ItemMasterSalesId, @ParentSubAssemblyId, @ChildSubAssemblyId, @ChildRawMaterialId, @Level, @PType); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

                    var id = await db.QueryAsync<int>(sql, new
                    {
                        ItemMasterSalesId = bomStructure.ItemMasterSalesId,
                        ParentSubAssemblyId = bomStructure.ParentSubAssemblyId,
                        ChildSubAssemblyId = bomStructure.ChildSubAssemblyId,
                        ChildRawMaterialId = bomStructure.ChildRawMaterialId,
                        Level = bomStructure.Level,
                        PType = bomStructure.PType
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

            var result = await db.QueryAsync<dynamic>(storedProcedureName,
                                                       new { Id = id },
                                                       commandType: CommandType.StoredProcedure
                                                       ).ConfigureAwait(false);

            return await GetBOMStructureObjectFromResult(result.SingleOrDefault());
        }

        public async Task<IEnumerable<BOMStructure>> GetAllBOMStructuresAsync()
        {
            const string storedProcedureName = "dbo.GetAllBOMStructures";

            var result = await db.QueryAsync<dynamic>(storedProcedureName,
                                                       commandType: CommandType.StoredProcedure
                                                       ).ConfigureAwait(false);

            var bomStructuresList = new List<BOMStructure>();
            foreach (var item in result)
            {
                var bomStructure = await GetBOMStructureObjectFromResult(item);
                bomStructuresList.Add(bomStructure);
            }

            return bomStructuresList;
        }

        public async Task<BOMStructure> UpdateBOMStructureAsync(BOMStructure bomStructure)
        {
            const string storedProcedureName = "dbo.UpdateBOMStructure";

            await db.ExecuteAsync(storedProcedureName,
                                  new
                                  {
                                      BOMId = bomStructure.Id,
                                      NewItemMasterSalesId = bomStructure.ItemMasterSalesId,
                                      NewParentSubAssemblyId = bomStructure.ParentSubAssemblyId,
                                      NewChildSubAssemblyId = bomStructure.ChildSubAssemblyId,
                                      NewChildRawMaterialId = bomStructure.ChildRawMaterialId,
                                      NewLevel = bomStructure.Level,
                                      NewPType = bomStructure.PType
                                  },
                                  commandType: CommandType.StoredProcedure
                                  ).ConfigureAwait(false);

            return bomStructure;
        }

        public async Task DeleteBOMStructureAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteBOMStructureById";

            await db.ExecuteAsync(storedProcedureName,
                                  new { Id = id },
                                  commandType: CommandType.StoredProcedure
                                  ).ConfigureAwait(false);
        }

        private async Task<BOMStructure> GetBOMStructureObjectFromResult(dynamic result)
        {
            BOMStructure bomStructure = new BOMStructure();
            bomStructure.Id = result.ID;
            bomStructure.ItemMasterSalesId = result.ItemMasterSalesID;
            bomStructure.ParentSubAssemblyId = result?.ParentSubAssemblyId;
            bomStructure.ChildSubAssemblyId = result?.ChildSubAssemblyId;
            bomStructure.ChildRawMaterialId = result.ChildRawMaterialID;
            bomStructure.Level = result.Level;
            bomStructure.PType = result.PType;

            return bomStructure;
        }
    }
}
