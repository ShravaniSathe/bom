using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.Repositories.ItemMasterSales.Abstractions;
using Dapper;

namespace bom.Repositories.ItemMasterSales.Implementations
{
    public class ItemMasterSalesRepository : Repository<ItemMasterSale> , IItemMasterSalesRepository
    {
        public ItemMasterSalesRepository(string connectionString) :
            base(connectionString)
        {

        }
        public async Task<ItemMasterSale> AddItemMasterSalesAsync(ItemMasterSale itemMasterSale)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    var sql = "INSERT INTO dbo.ItemMasterSales (ItemName, ItemCode, Grade, UOM, Quantity, Level, PType, CreatedDate) " +
                              "VALUES (@ItemName, @ItemCode, @Grade, @UOM, @Quantity, @Level, @PType, @CreatedDate); " +
                              "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await this.db.QueryAsync<int>(sql, new
                    {
                        ItemName = itemMasterSale.ItemName,
                        ItemCode = itemMasterSale.ItemCode,
                        Grade = itemMasterSale.Grade,
                        UOM = itemMasterSale.UOM,
                        Quantity = itemMasterSale.Quantity,
                        Level = itemMasterSale.Level,
                        PType = itemMasterSale.PType,
                        CreatedDate = itemMasterSale.CreatedDate
                    }, transaction: tran);

                    itemMasterSale.Id = id.Single();

                    CommitTransaction(tran);

                    return itemMasterSale;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw; 
                }
            }
        }

        public async Task<bool> DeleteItemMasterSalesAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteItemMasterSalesById"; 

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new { Id = id },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }


        public async Task<List<ItemMasterSale>> GetAllItemMasterSalesAsync()
        {
            const string storedProcedureName = "dbo.GetAllItemMasterSales"; 

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            List<ItemMasterSale> itemMasterSalesList = new List<ItemMasterSale>();
            foreach (var rawItemMasterSaleObj in retVal)
            {
                ItemMasterSale itemMasterSale = await GetItemMasterSaleObjectFromResult(rawItemMasterSaleObj);
                itemMasterSalesList.Add(itemMasterSale);
            }

            return itemMasterSalesList;
        }


        public async Task<ItemMasterSale> GetItemMasterSalesAsync(int id)
        {
            const string storedProcedureName = "dbo.GetItemMasterSaleById";

            var result = await db.QueryAsync(storedProcedureName,
                                             new { Id = id },
                                             commandType: CommandType.StoredProcedure
                                             ).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                ItemMasterSale itemMasterSale = await GetItemMasterSaleObjectFromResult(result.FirstOrDefault()).ConfigureAwait(false);
                return itemMasterSale;
            }
            else
            {
                return null;
            }
        }



        public async Task<ItemMasterSale> UpdateItemMasterSalesAsync(ItemMasterSale itemMasterSale)
        {
            string storedProcedureName = "dbo.UpdateItemMasterSales"; 

            await db.QueryAsync(storedProcedureName,
                                new
                                {
                                    ItemId = itemMasterSale.Id,
                                    NewItemName = itemMasterSale.ItemName,
                                    NewItemCode = itemMasterSale.ItemCode,
                                    NewGrade = itemMasterSale.Grade,
                                    NewUOM = itemMasterSale.UOM,
                                    NewQuantity = itemMasterSale.Quantity,
                                    NewLevel = itemMasterSale.Level,
                                    NewPType = itemMasterSale.PType,
                                    NewCreatedDate = itemMasterSale.CreatedDate
                                },
                                commandType: CommandType.StoredProcedure
                                ).ConfigureAwait(false);

            return itemMasterSale; 
        }

        private async Task<ItemMasterSale> GetItemMasterSaleObjectFromResult(dynamic rawItemMasterSaleObject)
        {
            ItemMasterSale itemMasterSale = new ItemMasterSale();


                itemMasterSale.Id = rawItemMasterSaleObject?.Id ?? 0;
                itemMasterSale.ItemName = rawItemMasterSaleObject?.ItemName ?? string.Empty;
                itemMasterSale.ItemCode = rawItemMasterSaleObject?.ItemCode ?? string.Empty;
                itemMasterSale.Grade = rawItemMasterSaleObject?.Grade ?? string.Empty;
                itemMasterSale.UOM = rawItemMasterSaleObject?.UOM ?? string.Empty;
                itemMasterSale.Quantity = rawItemMasterSaleObject?.Quantity ?? 0;
                itemMasterSale.Level = rawItemMasterSaleObject?.Level ?? 0;
                itemMasterSale.PType = rawItemMasterSaleObject?.PType ?? string.Empty;
                itemMasterSale.CreatedDate = rawItemMasterSaleObject?.CreatedDate ?? DateTime.MinValue;
            

            return itemMasterSale;
        }


    }
}
