using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Managers.ItemRawMaterialMappings.Abstractions;
using bom.Repositories.ItemRawMaterialMappings.Abstractions;
using bom.Models.ItemMasterSales;

namespace bom.Managers.ItemRawMaterialMappings.Implementations
{
    public class ItemRawMaterialMappingManager : IItemRawMaterialMappingManager
    {
        private readonly IItemRawMaterialMappingRepository _repo;
        private string connectionString;

        public ItemRawMaterialMappingManager(IItemRawMaterialMappingRepository repo)
        {
            _repo = repo;
        }

        public ItemRawMaterialMappingManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<ItemRawMaterialMapping> AddItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping)
        {
            return await _repo.AddItemRawMaterialMappingAsync(itemRawMaterialMapping);
        }

        public async Task DeleteItemRawMaterialMappingAsync(int id)
        {
            await _repo.DeleteItemRawMaterialMappingAsync(id);
        }

        public async Task<List<ItemRawMaterialMapping>> GetAllItemRawMaterialMappingsAsync()
        {
            return await _repo.GetAllItemRawMaterialMappingsAsync();
        }

        public async Task<ItemRawMaterialMapping> GetItemRawMaterialMappingAsync(int id)
        {
            return await _repo.GetItemRawMaterialMappingAsync(id);
        }

        public async Task<ItemRawMaterialMapping> UpdateItemRawMaterialMappingAsync(ItemRawMaterialMapping itemRawMaterialMapping)
        {
            return await _repo.UpdateItemRawMaterialMappingAsync(itemRawMaterialMapping);
        }
    }
}
