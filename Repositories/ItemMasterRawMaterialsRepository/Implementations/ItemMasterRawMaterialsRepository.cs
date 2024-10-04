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
                    const string sql = "INSERT INTO dbo.ItemMasterRawMaterials (SubAssemblyId, ItemName, ItemCode, Grade, UOM, Quantity, Level, PType, CostPerUnit) " +
                                       "VALUES (@SubAssemblyId, @ItemName, @ItemCode, @Grade, @UOM, @Quantity, @Level, @PType, @CostPerUnit); " +
                                       "SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = await db.QuerySingleAsync<int>(sql, new
                    {
                        SubAssemblyId = itemMasterRawMaterial.SubAssemblyId,
                        itemMasterRawMaterial.ItemName,
                        itemMasterRawMaterial.ItemCode,
                        itemMasterRawMaterial.Grade,
                        itemMasterRawMaterial.UOM,
                        itemMasterRawMaterial.Quantity,
                        itemMasterRawMaterial.Level,
                        itemMasterRawMaterial.PType,
                        itemMasterRawMaterial.CostPerUnit
                    }, transaction: tran);

                    itemMasterRawMaterial.Id = id;

                    CommitTransaction(tran);

                    return itemMasterRawMaterial;
                }
                catch (Exception ex)
                {
                    RollbackTransaction(tran);
                    throw new Exception("Error adding ItemMasterRawMaterial", ex);
                }
            }
        }

        public async Task<bool> DeleteItemMasterRawMaterialAsync(int id)
        {
            const string storedProcedureName = "dbo.DeleteItemMasterRawMaterialById";

            var result = await db.ExecuteAsync(storedProcedureName, new { Id = id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<List<ItemMasterRawMaterial>> GetAllItemMasterRawMaterialsAsync()
        {
            const string storedProcedureName = "dbo.GetAllItemMasterRawMaterials";

            var result = await db.QueryAsync(storedProcedureName,
                                              commandType: CommandType.StoredProcedure
                                              ).ConfigureAwait(false);

            var retVal = result.ToList();

            var itemMasterRawMaterialsList = new List<ItemMasterRawMaterial>();
            foreach (var item in retVal)
            {
                ItemMasterRawMaterial itemMasterRawMaterial = await GetItemMasterRawMaterialObjectFromResult(item);
                itemMasterRawMaterialsList.Add(itemMasterRawMaterial);
            }

            return itemMasterRawMaterialsList;
        }

        public async Task<ItemMasterRawMaterial> GetItemMasterRawMaterialAsync(int id)
        {
            const string storedProcedureName = "dbo.GetItemMasterRawMaterialById";

            var result = await db.QueryAsync<dynamic>(storedProcedureName, new { Id = id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return GetItemMasterRawMaterialObjectFromResult(result.SingleOrDefault());
        }

        public async Task<ItemMasterRawMaterial> UpdateItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial)
        {
            const string storedProcedureName = "dbo.UpdateItemMasterRawMaterial";

            await db.ExecuteAsync(storedProcedureName, new
            {
                NewSubAssemblyId = itemMasterRawMaterial.SubAssemblyId,
                NewItemName = itemMasterRawMaterial.ItemName,
                NewItemCode = itemMasterRawMaterial.ItemCode,
                NewGrade = itemMasterRawMaterial.Grade,
                NewUOM = itemMasterRawMaterial.UOM,
                NewQuantity = itemMasterRawMaterial.Quantity,
                NewLevel = itemMasterRawMaterial.Level,
                NewPType = itemMasterRawMaterial.PType,
                NewCostPerUnit = itemMasterRawMaterial.CostPerUnit
            }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            return itemMasterRawMaterial;
        }

        private ItemMasterRawMaterial GetItemMasterRawMaterialObjectFromResult(dynamic result)
        {
            if (result == null)
            {
                return null; 
            }

            return new ItemMasterRawMaterial
            {
                Id = result.Id,
                SubAssemblyId = result.SubAssemblyId,
                ItemName = result.ItemName,
                ItemCode = result.ItemCode,
                Grade = result.Grade,
                UOM = result.UOM,
                Quantity = result.Quantity,
                Level = result.Level,
                PType = result.PType,
                CostPerUnit = result.CostPerUnit
            };
        }
    }
}
