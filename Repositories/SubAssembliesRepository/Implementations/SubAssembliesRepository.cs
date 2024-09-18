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
                    const string sql = "INSERT INTO dbo.SubAssemblies (ItemName, UOM, CostPerUnit) " +
                                       "VALUES (@ItemName, @UOM, @CostPerUnit); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        ItemName = subAssembly.ItemName,
                        UOM = subAssembly.UOM,
                        CostPerUnit = subAssembly.CostPerUnit
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

        public async Task<bool> UpdateSubAssemblyAsync(SubAssemblie subAssembly)
        {
            const string storedProcedureName = "dbo.UpdateSubAssembly";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   Id = subAssembly.Id,
                                                   ItemName = subAssembly.ItemName,
                                                   UOM = subAssembly.UOM,
                                                   CostPerUnit = subAssembly.CostPerUnit
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        private async Task<SubAssemblie> GetSubAssemblieObjectFromResult(dynamic rawSubAssemblieObject)
        {
            SubAssemblie subAssemblie = new SubAssemblie
            {
                Id = rawSubAssemblieObject?.Id ?? 0,
                ItemName = rawSubAssemblieObject?.ItemName ?? string.Empty,
                UOM = rawSubAssemblieObject?.UOM ?? string.Empty,
                CostPerUnit = rawSubAssemblieObject?.CostPerUnit ?? 0
            };

            return subAssemblie;
        }
    }
}
