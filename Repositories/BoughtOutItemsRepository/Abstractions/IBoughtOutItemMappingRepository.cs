using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.BoughtOutItems;

namespace bom.Repositories.BoughtOutItems.Abstractions
{
    public interface IBoughtOutItemMappingRepository : IRepository<BoughtOutItemMapping>
    {
        Task<BoughtOutItemMapping> AddBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping);
        Task<BoughtOutItemMapping> GetBoughtOutItemMappingAsync(int id);
        Task<IEnumerable<BoughtOutItemMapping>> GetAllBoughtOutItemMappingsAsync();
        Task<bool> UpdateBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping);
        Task DeleteBoughtOutItemMappingAsync(int id);
    }
}
