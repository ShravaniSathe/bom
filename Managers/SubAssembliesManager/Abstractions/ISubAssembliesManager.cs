using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;

namespace bom.Managers.SubAssemblies.Abstractions
{
    public interface ISubAssembliesManager
    {
        Task<SubAssemblie> AddSubAssemblyAsync(SubAssemblie subAssembly);
        Task<SubAssemblie> GetSubAssemblyAsync(int id);
        Task<List<SubAssemblie>> GetAllSubAssembliesAsync();
        Task<SubAssemblie> UpdateSubAssemblyAsync(SubAssemblie subAssembly);
        Task DeleteSubAssemblyAsync(int id);
    }
}
