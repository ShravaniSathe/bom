using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Managers.BOMStructures.Abstractions;
using bom.Repositories.BOMStructures.Abstractions;
using bom.Models.BOMStructures;

namespace bom.Managers.BOMStructures.Implementations
{
    public class BOMStructuresManager : IBOMStructuresManager
    {
        private readonly IBOMStructuresRepository _bomStructuresRepository;

        public BOMStructuresManager(IBOMStructuresRepository bomStructuresRepository)
        {
            _bomStructuresRepository = bomStructuresRepository;
        }

        public async Task<BOMStructure> AddBOMStructureAsync(BOMStructure bomStructure)
        {
            return await _bomStructuresRepository.AddBOMStructureAsync(bomStructure);
        }

        public async Task<BOMStructure> GetBOMStructureAsync(int id)
        {
            return await _bomStructuresRepository.GetBOMStructureAsync(id);
        }

        public async Task<IEnumerable<BOMStructure>> GetAllBOMStructuresAsync()
        {
            return await _bomStructuresRepository.GetAllBOMStructuresAsync();
        }

        public async Task<bool> UpdateBOMStructureAsync(BOMStructure bomStructure)
        {
            return await _bomStructuresRepository.UpdateBOMStructureAsync(bomStructure);
        }

        public async Task DeleteBOMStructureAsync(int id)
        {
            await _bomStructuresRepository.DeleteBOMStructureAsync(id);
        }
    }
}
