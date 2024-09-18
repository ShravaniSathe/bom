using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.BOMStructures;

namespace bom.Repositories.BOMStructures.Abstractions
{
    public interface IBOMStructuresRepository : IRepository<BOMStructure>
    {
        Task<BOMStructure> AddBOMStructureAsync(BOMStructure bomStructure);
        Task<BOMStructure> GetBOMStructureAsync(int id);
        Task<IEnumerable<BOMStructure>> GetAllBOMStructuresAsync();
        Task<bool> UpdateBOMStructureAsync(BOMStructure bomStructure);
        Task DeleteBOMStructureAsync(int id);
    }
}
