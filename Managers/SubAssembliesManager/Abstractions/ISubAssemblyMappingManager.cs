using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;

namespace bom.Managers.SubAssemblies.Abstractions
{
    public interface ISubAssemblyMappingManager
    {
        Task<SubAssemblyMapping> AddSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping);
        Task<SubAssemblyMapping> GetSubAssemblyMappingAsync(int id);
        Task<List<SubAssemblyMapping>> GetAllSubAssemblyMappingsAsync();
        Task<bool> UpdateSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping);
        Task DeleteSubAssemblyMappingAsync(int id);
    }
}
