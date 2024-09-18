using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;

namespace bom.Managers.ItemMasterRawMaterials.Abstractions
{
    public interface IRawMaterialBoughtOutMappingManager
    {
        Task<RawMaterialBoughtOutMapping> AddRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping);
        Task<RawMaterialBoughtOutMapping> GetRawMaterialBoughtOutMappingAsync(int id);
        Task<List<RawMaterialBoughtOutMapping>> GetAllRawMaterialBoughtOutMappingsAsync();
        Task<RawMaterialBoughtOutMapping> UpdateRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping);
        Task DeleteRawMaterialBoughtOutMappingAsync(int id);
    }
}
