using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;

namespace bom.Repositories.ItemRawMaterialMappings.Abstractions
{
    public interface IItemRawMaterialMappingRepository : IRepository<ItemRawMaterialMapping>
    {
        Task<ItemRawMaterialMapping> AddItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping);
        Task<ItemRawMaterialMapping> GetItemRawMaterialMappingAsync(int id);
        Task<List<ItemRawMaterialMapping>> GetAllItemRawMaterialMappingsAsync();
        Task<ItemRawMaterialMapping> UpdateItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping);
        Task<bool> DeleteItemRawMaterialMappingAsync(int id);
    }
}
