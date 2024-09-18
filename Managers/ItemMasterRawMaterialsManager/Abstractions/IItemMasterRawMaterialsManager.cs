using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;

namespace bom.Managers.ItemMasterRawMaterials.Abstractions
{
    public interface IItemMasterRawMaterialsManager
    {
        Task<ItemMasterRawMaterial> AddItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial);
        Task<ItemMasterRawMaterial> GetItemMasterRawMaterialAsync(int id);
        Task<List<ItemMasterRawMaterial>> GetAllItemMasterRawMaterialsAsync();
        Task<ItemMasterRawMaterial> UpdateItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial);
        Task DeleteItemMasterRawMaterialAsync(int id);
    }
}
