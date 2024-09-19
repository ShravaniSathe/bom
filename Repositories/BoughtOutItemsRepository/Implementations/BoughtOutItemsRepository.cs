using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using bom.Models.BoughtOutItems;
using bom.Repositories.BoughtOutItems.Abstractions;

namespace bom.Repositories.BoughtOutItems.Implementations
{
    public class BoughtOutItemsRepository : Repository<BoughtOutItem>, IBoughtOutItemsRepository
    {
        public BoughtOutItemsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<BoughtOutItem> AddBoughtOutItemAsync(BoughtOutItem boughtOutItem)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.BoughtOutItems (ItemName, ItemCode, Grade, UOM, Quantity, Level, CostPerUnit) " +
                                       "VALUES (@ItemName, @ItemCode, @Grade, @UOM, @Quantity, @Level, @CostPerUnit); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        ItemName = boughtOutItem.ItemName,
                        ItemCode = boughtOutItem.ItemCode,
                        Grade = boughtOutItem.Grade,
                        UOM = boughtOutItem.UOM,
                        Quantity = boughtOutItem.Quantity,
                        Level = boughtOutItem.Level,
                        CostPerUnit = boughtOutItem.CostPerUnit
                    }, transaction: tran);

                    boughtOutItem.Id = id.Single();

                    CommitTransaction(tran);

                    return boughtOutItem;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<BoughtOutItem> GetBoughtOutItemAsync(int id)
        {
            const string storedProcedureName = "dbo.GetBoughtOutItemById";

            var result = await db.QueryAsync<dynamic>(storedProcedureName,
                                                       new { Id = id },
                                                       commandType: CommandType.StoredProcedure
                                                       ).ConfigureAwait(false);

            return await GetBoughtOutItemObjectFromResult(result.SingleOrDefault());
        }

        public async Task<IEnumerable<BoughtOutItem>> GetAllBoughtOutItemsAsync()
        {
            const string storedProcedureName = "dbo.GetAllBoughtOutItems";

            var result = await db.QueryAsync<dynamic>(storedProcedureName,
                                                       commandType: CommandType.StoredProcedure
                                                       ).ConfigureAwait(false);

            var boughtOutItemsList = new List<BoughtOutItem>();
            foreach (var item in result)
            {
                var boughtOutItem = await GetBoughtOutItemObjectFromResult(item);
                boughtOutItemsList.Add(boughtOutItem);
            }

            return boughtOutItemsList;
        }

        public async Task<BoughtOutItem> UpdateBoughtOutItemAsync(BoughtOutItem boughtOutItem)
        {
            const string storedProcedureName = "dbo.UpdateBoughtOutItem";

            await db.ExecuteAsync(storedProcedureName,
                                  new
                                  {
                                      Id = boughtOutItem.Id,
                                      NewItemName = boughtOutItem.ItemName,
                                      NewItemCode = boughtOutItem.ItemCode,
                                      NewGrade = boughtOutItem.Grade,
                                      NewUOM = boughtOutItem.UOM,
                                      NewQuantity = boughtOutItem.Quantity,
                                      NewLevel = boughtOutItem.Level,
                                      NewCostPerUnit = boughtOutItem.CostPerUnit
                                  },
                                  commandType: CommandType.StoredProcedure
                                  ).ConfigureAwait(false);

            return boughtOutItem;
        }

        public async Task DeleteBoughtOutItemAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteBoughtOutItemById";

            await db.ExecuteAsync(storedProcedureName,
                                  new { Id = id },
                                  commandType: CommandType.StoredProcedure
                                  ).ConfigureAwait(false);
        }

        private async Task<BoughtOutItem> GetBoughtOutItemObjectFromResult(dynamic result)
        {
            BoughtOutItem boughtOutItem = new BoughtOutItem
            {
                Id = result?.Id ?? 0,
                ItemName = result?.ItemName ?? string.Empty,
                ItemCode = result?.ItemCode ?? string.Empty,
                Grade = result?.Grade ?? string.Empty,
                UOM = result?.UOM ?? string.Empty,
                Quantity = result?.Quantity ?? 0,
                Level = result?.Level ?? string.Empty,
                CostPerUnit = result?.CostPerUnit ?? 0
            };

            return boughtOutItem;
        }
    }
}
