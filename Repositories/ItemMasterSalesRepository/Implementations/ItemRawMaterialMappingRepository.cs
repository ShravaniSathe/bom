using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;
using bom.Repositories.ItemMasterSales.Abstractions;
using Dapper;

namespace bom.Repositories.ItemMasterSales.Implementations
{
    public class ItemRawMaterialMappingRepository : Repository<ItemRawMaterialMapping>, IItemRawMaterialMappingRepository
    {
        public ItemRawMaterialMappingRepository(string connectionString) :
            base(connectionString)
        {


        }

        public async Task<ItemRawMaterialMapping> AddItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.ItemRawMaterialMapping (ItemMasterSalesId, ItemMasterRawMaterialId, Quantity, ProcureType) " +
                                       "VALUES (@ItemMasterSalesId, @ItemMasterRawMaterialId, @Quantity, @ProcureType); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        ItemMasterSalesId = itemRawMaterialMapping.ItemMasterSalesId,
                        ItemMasterRawMaterialId = itemRawMaterialMapping.ItemMasterRawMaterialId,
                        Quantity = itemRawMaterialMapping.Quantity,
                        ProcureType = itemRawMaterialMapping.ProcureType
                    }, transaction: tran);

                    itemRawMaterialMapping.Id = id.Single();

                    CommitTransaction(tran);

                    return itemRawMaterialMapping;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteItemRawMaterialMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteItemRawMaterialMappingById";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new { Id = id },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<List<ItemRawMaterialMapping>> GetAllItemRawMaterialMappingsAsync()
        {
            const string storedProcedureName = "dbo.GetAllItemRawMaterialMappings";

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            List<ItemRawMaterialMapping> itemRawMaterialMappingsList = new List<ItemRawMaterialMapping>();
            foreach (var rawItemRawMaterialMappingObj in retVal)
            {
                ItemRawMaterialMapping itemRawMaterialMapping = await GetItemRawMaterialMappingObjectFromResult(rawItemRawMaterialMappingObj);
                itemRawMaterialMappingsList.Add(itemRawMaterialMapping);
            }

            return itemRawMaterialMappingsList;
        }

        public async Task<ItemRawMaterialMapping> GetItemRawMaterialMappingAsync(int id)
        {
            const string storedProcedureName = "dbo.GetItemRawMaterialMappingById";

            var result = await db.QueryAsync(storedProcedureName,
                                             new { Id = id },
                                             commandType: CommandType.StoredProcedure
                                             ).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                ItemRawMaterialMapping itemRawMaterialMapping = await GetItemRawMaterialMappingObjectFromResult(result.FirstOrDefault()).ConfigureAwait(false);
                return itemRawMaterialMapping;
            }
            else
            {
                return null;
            }
        }

        public async Task<ItemRawMaterialMapping> UpdateItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping)
        {
            const string storedProcedureName = "dbo.UpdateItemRawMaterialMapping";

            await db.QueryAsync(storedProcedureName,
                                new
                                {
                                    ItemRawMaterialId = itemRawMaterialMapping.Id,
                                    NewItemMasterSalesId = itemRawMaterialMapping.ItemMasterSalesId,
                                    NewItemMasterRawMaterialId = itemRawMaterialMapping.ItemMasterRawMaterialId,
                                    NewQuantity = itemRawMaterialMapping.Quantity
                                },
                                commandType: CommandType.StoredProcedure
                                ).ConfigureAwait(false);

            return itemRawMaterialMapping;
        }

        private async Task<ItemRawMaterialMapping> GetItemRawMaterialMappingObjectFromResult(dynamic rawItemRawMaterialMappingObject)
        {
            ItemRawMaterialMapping itemRawMaterialMapping = new ItemRawMaterialMapping
            {
                Id = rawItemRawMaterialMappingObject?.Id ?? 0,
                ItemMasterSalesId = rawItemRawMaterialMappingObject?.ItemMasterSalesId ?? 0,
                ItemMasterRawMaterialId = rawItemRawMaterialMappingObject?.ItemMasterRawMaterialId ?? 0,
                Quantity = rawItemRawMaterialMappingObject?.Quantity ?? 0,
                ProcureType = rawItemRawMaterialMappingObject?.ProcureType
            };

            return itemRawMaterialMapping;
        }
    }
}
