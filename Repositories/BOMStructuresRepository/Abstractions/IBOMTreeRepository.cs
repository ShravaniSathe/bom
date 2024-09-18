using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.BOMStructure;

namespace bom.Repositories.BOMStructures.Abstractions
{
    public interface IBOMTreeRepository : IRepository<BOMTree>
    {
        Task<BOMTree> CreateBOMTreeAsync(BOMTree bomTree);
        Task<BOMTree> GetBOMTreeByIdAsync(int bomId);
        Task<BOMTree> UpdateBOMTreeAsync(BOMTree bomTree);
        Task DeleteBOMTreeAsync(int bomId);
    }
}
