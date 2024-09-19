using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Repositories.ItemMasterRawMaterials.Abstractions;
using Dapper;

namespace bom.Repositories.ItemMasterRawMaterials.Implementations
{
    public class ItemMasterRawMaterialsRepository : Repository<ItemMasterRawMaterial>, IItemMasterRawMaterialsRepository
    {
        public ItemMasterRawMaterialsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<ItemMasterRawMaterial> AddItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial)
        {
            using (var tran = BeginTransaction())
            {
                try
                {
                    const string sql = "INSERT INTO dbo.ItemMasterRawMaterials (ItemMasterSalesId, ItemName, ItemCode, Grade, UOM, Quantity, Level, PType, CostPerUnit) " +
                                       "VALUES (@ItemMasterSalesId, @ItemName, @ItemCode, @Grade, @UOM, @Quantity, @Level, @PType, @CostPerUnit); " +
                                       "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var id = await db.QueryAsync<int>(sql, new
                    {
                        ItemMasterSalesId = itemMasterRawMaterial.ItemMasterSalesId,
                        ItemName = itemMasterRawMaterial.ItemName,
                        ItemCode = itemMasterRawMaterial.ItemCode,
                        Grade = itemMasterRawMaterial.Grade,
                        UOM = itemMasterRawMaterial.UOM,
                        Quantity = itemMasterRawMaterial.Quantity,
                        Level = itemMasterRawMaterial.Level,
                        PType = itemMasterRawMaterial.PType,
                        CostPerUnit = itemMasterRawMaterial.CostPerUnit
                    }, transaction: tran);

                    itemMasterRawMaterial.Id = id.Single();

                    CommitTransaction(tran);

                    return itemMasterRawMaterial;
                }
                catch (Exception)
                {
                    RollbackTransaction(tran);
                    throw;
                }
            }
        }

        public async Task<bool> DeleteItemMasterRawMaterialAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteItemMasterRawMaterialById";

            var result = await db.ExecuteAsync(storedProcedureName,
                                               new { Id = id },
                                               commandType: CommandType.StoredProcedure
                                               ).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<List<ItemMasterRawMaterial>> GetAllItemMasterRawMaterialsAsync()
        {
            const string storedProcedureName = "dbo.GetAllItemMasterRawMaterials";

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            List<ItemMasterRawMaterial> itemMasterRawMaterialsList = new List<ItemMasterRawMaterial>();
            foreach (var rawItemMasterRawMaterialObj in retVal)
            {
                ItemMasterRawMaterial itemMasterRawMaterial = await GetItemMasterRawMaterialObjectFromResult(rawItemMasterRawMaterialObj);
                itemMasterRawMaterialsList.Add(itemMasterRawMaterial);
            }

            return itemMasterRawMaterialsList;
        }

        public async Task<ItemMasterRawMaterial> GetItemMasterRawMaterialAsync(int id)
        {
            const string storedProcedureName = "dbo.GetItemMasterRawMaterialById";

            var result = await db.QueryAsync(storedProcedureName,
                                             new { Id = id },
                                             commandType: CommandType.StoredProcedure
                                             ).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                ItemMasterRawMaterial itemMasterRawMaterial = await GetItemMasterRawMaterialObjectFromResult(result.FirstOrDefault()).ConfigureAwait(false);
                return itemMasterRawMaterial;
            }
            else
            {
                return null;
            }
        }

        public async Task<ItemMasterRawMaterial> UpdateItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial)
        {
            const string storedProcedureName = "dbo.UpdateItemMasterRawMaterial";

            await db.QueryAsync(storedProcedureName,
                                new
                                {
                                    ItemMasterRawMaterialId = itemMasterRawMaterial.Id,
                                    NewItemMasterSaleId = itemMasterRawMaterial.ItemMasterSalesId,
                                    NewItemName = itemMasterRawMaterial.ItemName,
                                    NewItemCode = itemMasterRawMaterial.ItemCode,
                                    NewGrade = itemMasterRawMaterial.Grade,
                                    NewUOM = itemMasterRawMaterial.UOM,
                                    NewQuantity = itemMasterRawMaterial.Quantity,
                                    NewLevel = itemMasterRawMaterial.Level,
                                    NewPType = itemMasterRawMaterial.PType,
                                    NewCostPerUnit = itemMasterRawMaterial.CostPerUnit
                                },
                                commandType: CommandType.StoredProcedure
                                ).ConfigureAwait(false);

            return itemMasterRawMaterial;
        }

        private async Task<ItemMasterRawMaterial> GetItemMasterRawMaterialObjectFromResult(dynamic rawItemMasterRawMaterialObject)
        {
            ItemMasterRawMaterial itemMasterRawMaterial = new ItemMasterRawMaterial
            {
                Id = rawItemMasterRawMaterialObject?.Id ?? 0,
                ItemMasterSalesId = rawItemMasterRawMaterialObject?.ItemMasterSalesId ?? 0,
                ItemName = rawItemMasterRawMaterialObject?.ItemName ?? string.Empty,
                ItemCode = rawItemMasterRawMaterialObject?.ItemCode ?? string.Empty,
                Grade = rawItemMasterRawMaterialObject?.Grade ?? string.Empty,
                UOM = rawItemMasterRawMaterialObject?.UOM ?? string.Empty,
                Quantity = rawItemMasterRawMaterialObject?.Quantity ?? 0,
                Level = rawItemMasterRawMaterialObject?.Level ?? 0,
                PType = rawItemMasterRawMaterialObject?.PType ?? string.Empty,
                CostPerUnit = rawItemMasterRawMaterialObject?.CostPerUnit ?? 0
            };

            return itemMasterRawMaterial;
        }
    }
}
