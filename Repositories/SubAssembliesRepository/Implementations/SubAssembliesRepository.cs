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
    public class SubAssembliesRepository : Repository<SubAssemblie>, ISubAssembliesRepository
    {
        public SubAssembliesRepository(string connectionString) :
            base(connectionString)
        {
        }

        public async Task<SubAssemblie> AddSubAssemblyAsync(SubAssemblie subAssembly)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.SubAssemblies (ItemMasterSalesId, ItemName, UOM, CostPerUnit, Level, ParentSubAssemblyId, PType) " +
                                       "VALUES (@ItemMasterSalesId, @ItemName, @UOM, @CostPerUnit, @Level, @ParentSubAssemblyId, @PType); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        ItemMasterSalesId = subAssembly.ItemMasterSalesId,
                        ItemName = subAssembly.ItemName,
                        UOM = subAssembly.UOM,
                        CostPerUnit = subAssembly.CostPerUnit,
                        Level = subAssembly.Level,
                        ParentSubAssemblyId = subAssembly.ParentSubAssemblyId,
                        PType = subAssembly.PType
                    }, transaction: tran);

                    subAssembly.Id = id.Single();

                    CommitTransaction(tran);

                    return subAssembly;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteSubAssemblyAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteSubAssemblyById";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new { Id = id },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<List<SubAssemblie>> GetAllSubAssembliesAsync()
        {
            const string storedProcedureName = "dbo.GetAllSubAssemblies";

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            List<SubAssemblie> subAssembliesList = new List<SubAssemblie>();
            foreach (var rawSubAssemblieObj in retVal)
            {
                SubAssemblie subAssemblie = await GetSubAssemblieObjectFromResult(rawSubAssemblieObj);
                subAssembliesList.Add(subAssemblie);
            }

            return subAssembliesList;
        }

        public async Task<SubAssemblie> GetSubAssemblyAsync(int id)
        {
            const string storedProcedureName = "dbo.GetSubAssemblyById";

            var result = await db.QueryAsync(storedProcedureName,
                                             new { Id = id },
                                             commandType: CommandType.StoredProcedure
                                             ).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                SubAssemblie subAssemblie = await GetSubAssemblieObjectFromResult(result.FirstOrDefault()).ConfigureAwait(false);
                return subAssemblie;
            }
            else
            {
                return null;
            }
        }

        public async Task<SubAssemblie> UpdateSubAssemblyAsync(SubAssemblie subAssembly)
        {
            const string storedProcedureName = "dbo.UpdateSubAssembly";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   SubAssemblyId = subAssembly.Id,
                                                   NewItemMasterSalesId = subAssembly.ItemMasterSalesId,
                                                   NewItemName = subAssembly.ItemName,
                                                   NewUOM = subAssembly.UOM,
                                                   NewCostPerUnit = subAssembly.CostPerUnit,
                                                   NewLevel = subAssembly.Level,
                                                   NewParentSubAssemblyId = subAssembly.ParentSubAssemblyId,
                                                   NewPType = subAssembly.PType
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return subAssembly;
        }

        private async Task<SubAssemblie> GetSubAssemblieObjectFromResult(dynamic rawSubAssemblieObject)
        {
            SubAssemblie subAssemblie = new SubAssemblie
            {
                Id = rawSubAssemblieObject?.Id ?? 0,
                ItemMasterSalesId = rawSubAssemblieObject?.ItemMasterSalesId ?? 0,
                ItemName = rawSubAssemblieObject?.ItemName ?? string.Empty,
                UOM = rawSubAssemblieObject?.UOM ?? string.Empty,
                CostPerUnit = rawSubAssemblieObject?.CostPerUnit ?? 0,
                Level = rawSubAssemblieObject?.Level ?? 0,
                ParentSubAssemblyId = rawSubAssemblieObject?.ParentSubAssemblyId,
                PType = rawSubAssemblieObject?.PType ?? string.Empty
            };

            return subAssemblie;
        }
    }
}
