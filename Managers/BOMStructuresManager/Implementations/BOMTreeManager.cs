using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bom.Managers.BOMStructures.Abstractions;
using bom.Models.BOMStructure;
using bom.Models.BOMStructures;
using bom.Repositories.BOMStructures.Abstractions;

namespace bom.Managers.BOMStructures
{
    public class BOMTreeManager : IBOMTreeManager
    {
        private readonly IBOMTreeRepository _bomTreeRepository;

        public BOMTreeManager(IBOMTreeRepository bomTreeRepository)
        {
            _bomTreeRepository = bomTreeRepository;
        }

        public async Task<BOMTree> CreateBOMTreeAsync(BOMTree bomTree)
        {
            // Implementation to create a BOM tree in the database
            // For example, insert BOM tree and nodes into the database
            await _bomTreeRepository.CreateBOMTreeAsync(bomTree);
            return bomTree;
        }

        public async Task<BOMTree> GetBOMTreeAsync(int bomId)
        {
            // Implementation to retrieve a BOM tree from the database
            // For example, fetch BOM tree and nodes based on bomId
            var bomTree = await _bomTreeRepository.GetBOMTreeByIdAsync(bomId);
            return bomTree;
        }

        public async Task<BOMTree> UpdateBOMTreeAsync(BOMTree bomTree)
        {
            // Implementation to update an existing BOM tree in the database
            // For example, update BOM tree and nodes based on BOMId
            var existingBOMTree = await _bomTreeRepository.GetBOMTreeByIdAsync(bomTree.BOMId);
            if (existingBOMTree == null)
            {
                return null; // BOM tree not found
            }

            await _bomTreeRepository.UpdateBOMTreeAsync(bomTree);
            return bomTree;
        }

        public async Task DeleteBOMTreeAsync(int bomId)
        {
            // Implementation to delete a BOM tree from the database
            // For example, delete BOM tree and nodes based on bomId
            await _bomTreeRepository.DeleteBOMTreeAsync(bomId);
        }
    }
}
