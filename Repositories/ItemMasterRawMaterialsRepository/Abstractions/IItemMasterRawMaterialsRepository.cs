using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;

namespace bom.Repositories.ItemMasterRawMaterials.Abstractions
{
    public interface IItemMasterRawMaterialsRepository : IRepository<ItemMasterRawMaterial>
    {
        Task<ItemMasterRawMaterial> AddItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial);
        Task<ItemMasterRawMaterial> GetItemMasterRawMaterialAsync(int id);
        Task<List<ItemMasterRawMaterial>> GetAllItemMasterRawMaterialsAsync();
        Task<ItemMasterRawMaterial> UpdateItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial);
        Task<bool> DeleteItemMasterRawMaterialAsync(int id);
    }
}
