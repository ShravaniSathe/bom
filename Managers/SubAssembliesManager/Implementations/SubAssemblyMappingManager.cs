using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;
using bom.Managers.SubAssemblies.Abstractions;
using bom.Repositories.SubAssemblies.Abstractions;

namespace bom.Managers.SubAssemblies.Implementations
{
    public class SubAssemblyMappingManager : ISubAssemblyMappingManager
    {
        private readonly ISubAssemblyMappingRepository _subAssemblyMappingRepository;
        private string connectionString;

        public SubAssemblyMappingManager(ISubAssemblyMappingRepository subAssemblyMappingRepository)
        {
            _subAssemblyMappingRepository = subAssemblyMappingRepository;
        }

        public SubAssemblyMappingManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<SubAssemblyMapping> AddSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping)
        {
            return await _subAssemblyMappingRepository.AddSubAssemblyMappingAsync(subAssemblyMapping);
        }

        public async Task DeleteSubAssemblyMappingAsync(int id)
        {
            await _subAssemblyMappingRepository.DeleteSubAssemblyMappingAsync(id);
        }

        public async Task<List<SubAssemblyMapping>> GetAllSubAssemblyMappingsAsync()
        {
            return await _subAssemblyMappingRepository.GetAllSubAssemblyMappingsAsync();
        }

        public async Task<SubAssemblyMapping> GetSubAssemblyMappingAsync(int id)
        {
            return await _subAssemblyMappingRepository.GetSubAssemblyMappingAsync(id);
        }

        public async Task<bool> UpdateSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping)
        {
            return await _subAssemblyMappingRepository.UpdateSubAssemblyMappingAsync(subAssemblyMapping);
        }
    }
}
