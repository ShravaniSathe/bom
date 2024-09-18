using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;

namespace bom.Repositories.SubAssemblies.Abstractions
{
    public interface ISubAssemblyMappingRepository : IRepository<SubAssemblyMapping>
    {
        Task<SubAssemblyMapping> AddSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping);
        Task<bool> DeleteSubAssemblyMappingAsync(int id);
        Task<List<SubAssemblyMapping>> GetAllSubAssemblyMappingsAsync();
        Task<SubAssemblyMapping> GetSubAssemblyMappingAsync(int id);
        Task<SubAssemblyMapping> UpdateSubAssemblyMappingAsync(SubAssemblyMapping subAssemblyMapping);
    }
}
