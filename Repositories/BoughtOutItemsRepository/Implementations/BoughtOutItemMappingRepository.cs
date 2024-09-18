using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using bom.Models.BoughtOutItems;
using bom.Repositories.BoughtOutItems.Abstractions;

namespace bom.Repositories.BoughtOutItems.Implementations
{
    public class BoughtOutItemMappingRepository : Repository<BoughtOutItemMapping>, IBoughtOutItemMappingRepository
    {
        public BoughtOutItemMappingRepository(string connectionString) : 
            base(connectionString)
        {


        }

    public async Task<BoughtOutItemMapping> AddBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.BoughtOutItemMappings (BoughtOutItemId, ItemId, Quantity) " +
                                       "VALUES (@BoughtOutItemId, @ItemId, @Quantity); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        boughtOutItemMapping.BoughtOutItemId,
                        boughtOutItemMapping.ItemId,
                        boughtOutItemMapping.Quantity
                    }, transaction: tran);

                    boughtOutItemMapping.BoughtOutItemId = id.Single();

                    CommitTransaction(tran);

                    return boughtOutItemMapping;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<BoughtOutItemMapping> GetBoughtOutItemMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.GetBoughtOutItemMappingById";

            var result = await db.QueryAsync<BoughtOutItemMapping>(storedProcedureName,
                                                                    new { Id = id },
                                                                    commandType: CommandType.StoredProcedure
                                                                    ).ConfigureAwait(false);

            return result.SingleOrDefault();
        }

        public async Task<IEnumerable<BoughtOutItemMapping>> GetAllBoughtOutItemMappingsAsync()
        {
            const string storedProcedureName = "dbo.GetAllBoughtOutItemMappings";

            var result = await db.QueryAsync<BoughtOutItemMapping>(storedProcedureName,
                                                                    commandType: CommandType.StoredProcedure
                                                                    ).ConfigureAwait(false);

            return result;
        }

        public async Task<BoughtOutItemMapping> UpdateBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping)
        {
            const string storedProcedureName = "dbo.UpdateBoughtOutItemMapping";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   Id = boughtOutItemMapping.BoughtOutItemId,
                                                   NewItemId = boughtOutItemMapping.ItemId,
                                                   NewQuantity = boughtOutItemMapping.Quantity
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return boughtOutItemMapping;
        }

        public async Task DeleteBoughtOutItemMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteBoughtOutItemMappingById";

            await db.ExecuteAsync(storedProcedureName,
                                  new { Id = id },
                                  commandType: CommandType.StoredProcedure
                                  ).ConfigureAwait(false);
        }
    }
}
