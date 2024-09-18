using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;

namespace bom.Repositories.RawMaterialBoughtOutMappings.Abstractions
{
    public interface IRawMaterialBoughtOutMappingRepository : IRepository<RawMaterialBoughtOutMapping>
    {
        Task<RawMaterialBoughtOutMapping> AddRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping);
        Task<bool> DeleteRawMaterialBoughtOutMappingAsync(int id);
        Task<List<RawMaterialBoughtOutMapping>> GetAllRawMaterialBoughtOutMappingsAsync();
        Task<RawMaterialBoughtOutMapping> GetRawMaterialBoughtOutMappingAsync(int id);
        Task<bool> UpdateRawMaterialBoughtOutMappingAsync(RawMaterialBoughtOutMapping mapping);
    }
}
