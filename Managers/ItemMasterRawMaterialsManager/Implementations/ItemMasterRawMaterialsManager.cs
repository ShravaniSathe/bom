using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Managers.ItemMasterRawMaterials.Abstractions;
using bom.Repositories.ItemMasterRawMaterials.Abstractions;

namespace bom.Managers.ItemMasterRawMaterials.Implementations
{
    public class ItemMasterRawMaterialsManager : IItemMasterRawMaterialsManager
    {
        private readonly IItemMasterRawMaterialsRepository _repo;

        public ItemMasterRawMaterialsManager(IItemMasterRawMaterialsRepository repo)
        {
            _repo = repo;
        }

        public async Task<ItemMasterRawMaterial> AddItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial)
        {
            return await _repo.AddItemMasterRawMaterialAsync(itemMasterRawMaterial);
        }

        public async Task DeleteItemMasterRawMaterialAsync(int id)
        {
            await _repo.DeleteItemMasterRawMaterialAsync(id);
        }

        public async Task<List<ItemMasterRawMaterial>> GetAllItemMasterRawMaterialsAsync()
        {
            return await _repo.GetAllItemMasterRawMaterialsAsync();
        }

        public async Task<ItemMasterRawMaterial> GetItemMasterRawMaterialAsync(int id)
        {
            return await _repo.GetItemMasterRawMaterialAsync(id);
        }

        public async Task<ItemMasterRawMaterial> UpdateItemMasterRawMaterialAsync(ItemMasterRawMaterial itemMasterRawMaterial)
        {
            return await _repo.UpdateItemMasterRawMaterialAsync(itemMasterRawMaterial);
        }
    }
}
