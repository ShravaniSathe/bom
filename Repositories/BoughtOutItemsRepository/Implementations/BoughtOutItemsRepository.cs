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
                        boughtOutItem.ItemName,
                        boughtOutItem.ItemCode,
                        boughtOutItem.Grade,
                        boughtOutItem.UOM,
                        boughtOutItem.Quantity,
                        boughtOutItem.Level,
                        boughtOutItem.CostPerUnit
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

            var result = await db.QueryAsync<BoughtOutItem>(storedProcedureName,
                                                             new { Id = id },
                                                             commandType: CommandType.StoredProcedure
                                                             ).ConfigureAwait(false);

            return result.SingleOrDefault();
        }

        public async Task<IEnumerable<BoughtOutItem>> GetAllBoughtOutItemsAsync()
        {
            const string storedProcedureName = "dbo.GetAllBoughtOutItems";

            var result = await db.QueryAsync<BoughtOutItem>(storedProcedureName,
                                                             commandType: CommandType.StoredProcedure
                                                             ).ConfigureAwait(false);

            return result;
        }

        public async Task<bool> UpdateBoughtOutItemAsync(BoughtOutItem boughtOutItem)
        {
            const string storedProcedureName = "dbo.UpdateBoughtOutItem";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new
                                               {
                                                   boughtOutItem.Id,
                                                   boughtOutItem.ItemName,
                                                   boughtOutItem.ItemCode,
                                                   boughtOutItem.Grade,
                                                   boughtOutItem.UOM,
                                                   boughtOutItem.Quantity,
                                                   boughtOutItem.Level,
                                                   boughtOutItem.CostPerUnit
                                               },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task DeleteBoughtOutItemAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteBoughtOutItemById";

            await db.ExecuteAsync(storedProcedureName,
                                  new { Id = id },
                                  commandType: CommandType.StoredProcedure
                                  ).ConfigureAwait(false);
        }
    }
}
