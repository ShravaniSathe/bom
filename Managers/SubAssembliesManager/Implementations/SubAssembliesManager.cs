using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;
using bom.Managers.SubAssemblies.Abstractions;
using bom.Repositories.SubAssemblies.Abstractions;

namespace bom.Managers.SubAssemblies.Implementations
{
    public class SubAssembliesManager : ISubAssembliesManager
    {
        private readonly ISubAssembliesRepository _subAssembliesRepository;

        public SubAssembliesManager(ISubAssembliesRepository subAssembliesRepository)
        {
            _subAssembliesRepository = subAssembliesRepository;
        }

        public async Task<SubAssemblie> AddSubAssemblyAsync(SubAssemblie subAssembly)
        {
            return await _subAssembliesRepository.AddSubAssemblyAsync(subAssembly);
        }

        public async Task DeleteSubAssemblyAsync(int id)
        {
            await _subAssembliesRepository.DeleteSubAssemblyAsync(id);
        }

        public async Task<List<SubAssemblie>> GetAllSubAssembliesAsync()
        {
            return await _subAssembliesRepository.GetAllSubAssembliesAsync();
        }

        public async Task<SubAssemblie> GetSubAssemblyAsync(int id)
        {
            return await _subAssembliesRepository.GetSubAssemblyAsync(id);
        }

        public async Task<SubAssemblie> UpdateSubAssemblyAsync(SubAssemblie subAssembly)
        {
            return await _subAssembliesRepository.UpdateSubAssemblyAsync(subAssembly);
        }
    }
}
