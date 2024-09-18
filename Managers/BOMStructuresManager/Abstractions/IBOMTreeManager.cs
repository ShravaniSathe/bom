using bom.Models.BOMStructure;

namespace bom.Managers.BOMStructures.Abstractions
{
    public interface IBOMTreeManager
    {
        Task<BOMTree> CreateBOMTreeAsync(BOMTree bomTree);
        Task<BOMTree> GetBOMTreeAsync(int bomId);
        Task<BOMTree> UpdateBOMTreeAsync(BOMTree bomTree);
        Task DeleteBOMTreeAsync(int bomId);
    }
}
