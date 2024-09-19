using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Managers.ItemMasterRawMaterials.Abstractions;
using bom.Repositories.ItemMasterRawMaterials.Abstractions;

namespace bom.Managers.ItemMasterRawMaterials.Implementations
{
    public class RawMaterialBoughtOutMappingManager : IRawMaterialBoughtOutMappingManager
    {
        private readonly IRawMaterialBoughtOutMappingRepository _repo;
       

        public RawMaterialBoughtOutMappingManager(IRawMaterialBoughtOutMappingRepository repo)
        {
            _repo = repo;
        }

        public async Task<RawMaterialBoughtOutMapping> AddRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping)
        {
            return await _repo.AddRawMaterialBoughtOutMappingAsync(mapping);
        }

        public async Task DeleteRawMaterialBoughtOutMappingAsync(int id)
        {
            await _repo.DeleteRawMaterialBoughtOutMappingAsync(id);
        }

        public async Task<List<RawMaterialBoughtOutMapping>> GetAllRawMaterialBoughtOutMappingsAsync()
        {
            return await _repo.GetAllRawMaterialBoughtOutMappingsAsync();
        }

        public async Task<RawMaterialBoughtOutMapping> GetRawMaterialBoughtOutMappingAsync(int id)
        {
            return await _repo.GetRawMaterialBoughtOutMappingAsync(id);
        }

        public async Task<RawMaterialBoughtOutMapping> UpdateRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping)
        {
            return await _repo.UpdateRawMaterialBoughtOutMappingAsync(mapping);
        }
    }
}
