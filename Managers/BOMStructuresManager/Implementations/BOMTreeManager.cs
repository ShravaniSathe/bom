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
            await _bomTreeRepository.CreateBOMTreeAsync(bomTree);
            return bomTree;
        }

        public async Task<BOMTree> GetBOMTreeAsync(int bomId)
        {
            var bomTree = await _bomTreeRepository.GetBOMTreeByIdAsync(bomId);
            return bomTree;
        }

        public async Task<BOMTree> UpdateBOMTreeAsync(BOMTree bomTree)
        {
            var existingBOMTree = await _bomTreeRepository.GetBOMTreeByIdAsync(bomTree.BOMId);
            if (existingBOMTree == null)
            {
                return null; 
            }

            await _bomTreeRepository.UpdateBOMTreeAsync(bomTree);
            return bomTree;
        }

        public async Task DeleteBOMTreeAsync(int bomId)
        {
            await _bomTreeRepository.DeleteBOMTreeAsync(bomId);
        }
    }
}
