using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.BOMStructures;

namespace bom.Managers.BOMStructures.Abstractions
{
    public interface IBOMStructuresManager
    {
        Task<BOMStructure> AddBOMStructureAsync(BOMStructure bomStructure);
        Task<BOMStructure> GetBOMStructureAsync(int id);
        Task<IEnumerable<BOMStructure>> GetAllBOMStructuresAsync();
        Task<BOMStructure> UpdateBOMStructureAsync(BOMStructure bomStructure);
        Task DeleteBOMStructureAsync(int id);
    }
}
