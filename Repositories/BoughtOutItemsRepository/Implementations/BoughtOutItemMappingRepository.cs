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
        public BoughtOutItemMappingRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<BoughtOutItemMapping> AddBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.BoughtOutItemMapping (BoughtOutItemId, ItemId, Quantity) " +
                                       "VALUES (@BoughtOutItemId, @ItemId, @Quantity); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        BoughtOutItemId = boughtOutItemMapping.BoughtOutItemId,
                        ItemId = boughtOutItemMapping.ItemId,
                        Quantity = boughtOutItemMapping.Quantity
                    }, transaction: tran);

                    boughtOutItemMapping.Id = id.Single();

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

            var result = await db.QueryAsync<dynamic>(storedProcedureName,
                                                       new { Id = id },
                                                       commandType: CommandType.StoredProcedure
                                                       ).ConfigureAwait(false);

            return await GetBoughtOutItemMappingObjectFromResult(result.SingleOrDefault());
        }

        public async Task<IEnumerable<BoughtOutItemMapping>> GetAllBoughtOutItemMappingsAsync()
        {
            const string storedProcedureName = "dbo.GetAllBoughtOutItemMappings";

            var result = await db.QueryAsync<dynamic>(storedProcedureName,
                                                       commandType: CommandType.StoredProcedure
                                                       ).ConfigureAwait(false);

            var boughtOutItemMappingsList = new List<BoughtOutItemMapping>();
            foreach (var item in result)
            {
                var boughtOutItemMapping = await GetBoughtOutItemMappingObjectFromResult(item);
                boughtOutItemMappingsList.Add(boughtOutItemMapping);
            }

            return boughtOutItemMappingsList;
        }

        public async Task<BoughtOutItemMapping> UpdateBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping)
        {
            const string storedProcedureName = "dbo.UpdateBoughtOutItemMapping";

            await db.ExecuteAsync(storedProcedureName,
                                  new
                                  {
                                      Id = boughtOutItemMapping.Id,
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

        private async Task<BoughtOutItemMapping> GetBoughtOutItemMappingObjectFromResult(dynamic result)
        {
            BoughtOutItemMapping boughtOutItemMapping = new BoughtOutItemMapping
            {
                Id = result?.Id ?? 0,
                BoughtOutItemId = result?.BoughtOutItemId ?? 0,
                ItemId = result?.ItemId ?? 0,
                Quantity = result?.Quantity ?? 0
            };

            return boughtOutItemMapping;
        }
    }
}
