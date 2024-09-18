using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.Models.SubAssemblies;

namespace bom.Repositories.SubAssemblies.Abstractions
{
    public interface ISubAssembliesRepository : IRepository<SubAssemblie>
    {
        Task<SubAssemblie> AddSubAssemblyAsync(SubAssemblie subAssembly);
        Task<bool> DeleteSubAssemblyAsync(int id);
        Task<List<SubAssemblie>> GetAllSubAssembliesAsync();
        Task<SubAssemblie> GetSubAssemblyAsync(int id);
        Task<SubAssemblie> UpdateSubAssemblyAsync(SubAssemblie subAssembly);
    }
}
