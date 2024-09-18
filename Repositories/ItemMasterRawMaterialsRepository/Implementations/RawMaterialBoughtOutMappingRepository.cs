using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Repositories.RawMaterialBoughtOutMappings.Abstractions;
using Dapper;

namespace bom.Repositories.RawMaterialBoughtOutMappings.Implementations
{
    public class RawMaterialBoughtOutMappingRepository : Repository<RawMaterialBoughtOutMapping>, IRawMaterialBoughtOutMappingRepository
    {
        public RawMaterialBoughtOutMappingRepository(string connectionString) :
            base(connectionString)
        {
        }

        public async Task<RawMaterialBoughtOutMapping> AddRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.RawMaterialBoughtOutMappings (ItemMasterRawMaterialId, BoughtOutId, CostPerUnit) " +
                                       "VALUES (@ItemMasterRawMaterialId, @BoughtOutId, @CostPerUnit); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        ItemMasterRawMaterialId = mapping.ItemMasterRawMaterialId,
                        BoughtOutId = mapping.BoughtOutId,
                        CostPerUnit = mapping.CostPerUnit
                    }, transaction: tran);

                    mapping.Id = id.Single();

                    CommitTransaction(tran);

                    return mapping;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteRawMaterialBoughtOutMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteRawMaterialBoughtOutMappingById";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new { Id = id },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<List<RawMaterialBoughtOutMapping>> GetAllRawMaterialBoughtOutMappingsAsync()
        {
            const string storedProcedureName = "dbo.GetAllRawMaterialBoughtOutMappings";

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            List<RawMaterialBoughtOutMapping> mappingsList = new List<RawMaterialBoughtOutMapping>();
            foreach (var rawMappingObj in retVal)
            {
                RawMaterialBoughtOutMapping mapping = await GetRawMaterialBoughtOutMappingObjectFromResult(rawMappingObj);
                mappingsList.Add(mapping);
            }

            return mappingsList;
        }

        public async Task<RawMaterialBoughtOutMapping> GetRawMaterialBoughtOutMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.GetRawMaterialBoughtOutMappingById";

            var result = await db.QueryAsync(storedProcedureName,
                                             new { Id = id },
                                             commandType: CommandType.StoredProcedure
                                             ).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                RawMaterialBoughtOutMapping mapping = await GetRawMaterialBoughtOutMappingObjectFromResult(result.FirstOrDefault()).ConfigureAwait(false);
                return mapping;
            }
            else
            {
                return null;
            }
        }

        public async Task<RawMaterialBoughtOutMapping> UpdateRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping)
        {
            const string storedProcedureName = "dbo.UpdateRawMaterialBoughtOutMapping";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   RawMaterialBoughtOutId = mapping.Id,
                                                   NewItemMasterRawMaterialId = mapping.ItemMasterRawMaterialId,
                                                   NewBoughtOutId = mapping.BoughtOutId,
                                                   NewCostPerUnit = mapping.CostPerUnit
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return mapping;
        }

        private async Task<RawMaterialBoughtOutMapping> GetRawMaterialBoughtOutMappingObjectFromResult(dynamic rawMappingObject)
        {
            RawMaterialBoughtOutMapping mapping = new RawMaterialBoughtOutMapping
            {
                Id = rawMappingObject?.Id ?? 0,
                ItemMasterRawMaterialId = rawMappingObject?.ItemMasterRawMaterialId ?? 0,
                BoughtOutId = rawMappingObject?.BoughtOutId ?? 0,
                CostPerUnit = rawMappingObject?.CostPerUnit ?? 0
            };

            return mapping;
        }
    }
}
