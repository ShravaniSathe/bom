using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;

namespace bom.Managers.ItemRawMaterialMappings.Abstractions
{
    public interface IItemRawMaterialMappingManager
    {
        Task<ItemRawMaterialMapping> AddItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping);
        Task<ItemRawMaterialMapping> GetItemRawMaterialMappingAsync(int id);
        Task<List<ItemRawMaterialMapping>> GetAllItemRawMaterialMappingsAsync();
        Task<ItemRawMaterialMapping> UpdateItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping);
        Task DeleteItemRawMaterialMappingAsync(int id);
    }
}
