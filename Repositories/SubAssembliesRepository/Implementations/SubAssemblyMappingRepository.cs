using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;
using bom.Repositories.SubAssemblies.Abstractions;
using Dapper;

namespace bom.Repositories.SubAssemblies.Implementations
{
    public class SubAssemblyMappingRepository : Repository<SubAssemblyMapping>, ISubAssemblyMappingRepository
    {
        public SubAssemblyMappingRepository(string connectionString) :
            base(connectionString)
        {
        }

        public async Task<SubAssemblyMapping> AddSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.SubAssemblyMappings (SubAssemblyId, ItemId, RawMaterialId, Quantity) " +
                                       "VALUES (@SubAssemblyId, @ItemId, @RawMaterialId, @Quantity); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        SubAssemblyId = subAssemblyMapping.SubAssemblyId,
                        ItemId = subAssemblyMapping.ItemId,
                        RawMaterialId = subAssemblyMapping.RawMaterialId,
                        Quantity = subAssemblyMapping.Quantity
                    }, transaction: tran);

                    // You might need to set the Id property if you have one for SubAssemblyMapping.
                    // subAssemblyMapping.Id = id.Single(); // Uncomment if Id is used.

                    CommitTransaction(tran);

                    return subAssemblyMapping;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteSubAssemblyMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteSubAssemblyMappingById";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new { Id = id },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<List<SubAssemblyMapping>> GetAllSubAssemblyMappingsAsync()
        {
            const string storedProcedureName = "dbo.GetAllSubAssemblyMappings";

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            List<SubAssemblyMapping> subAssemblyMappingsList = new List<SubAssemblyMapping>();
            foreach (var rawSubAssemblyMappingObj in retVal)
            {
                SubAssemblyMapping subAssemblyMapping = await GetSubAssemblyMappingObjectFromResult(rawSubAssemblyMappingObj);
                subAssemblyMappingsList.Add(subAssemblyMapping);
            }

            return subAssemblyMappingsList;
        }

        public async Task<SubAssemblyMapping> GetSubAssemblyMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.GetSubAssemblyMappingById";

            var result = await db.QueryAsync(storedProcedureName,
                                             new { Id = id },
                                             commandType: CommandType.StoredProcedure
                                             ).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                SubAssemblyMapping subAssemblyMapping = await GetSubAssemblyMappingObjectFromResult(result.FirstOrDefault()).ConfigureAwait(false);
                return subAssemblyMapping;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping)
        {
            const string storedProcedureName = "dbo.UpdateSubAssemblyMapping";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   SubAssemblyId = subAssemblyMapping.SubAssemblyId,
                                                   ItemId = subAssemblyMapping.ItemId,
                                                   RawMaterialId = subAssemblyMapping.RawMaterialId,
                                                   Quantity = subAssemblyMapping.Quantity
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        private async Task<SubAssemblyMapping> GetSubAssemblyMappingObjectFromResult(dynamic rawSubAssemblyMappingObject)
        {
            SubAssemblyMapping subAssemblyMapping = new SubAssemblyMapping
            {
                SubAssemblyId = rawSubAssemblyMappingObject?.SubAssemblyId ?? 0,
                ItemId = rawSubAssemblyMappingObject?.ItemId ?? 0,
                RawMaterialId = rawSubAssemblyMappingObject?.RawMaterialId,
                Quantity = rawSubAssemblyMappingObject?.Quantity ?? 0,
                // Mapping additional entities can be done here if needed
                // SubAssemblie = ...
                // ItemMasterSale = ...
                // ItemMasterRawMaterial = ...
            };

            return subAssemblyMapping;
        }
    }
}
