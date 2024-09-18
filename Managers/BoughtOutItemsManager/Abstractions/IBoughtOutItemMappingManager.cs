using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.BoughtOutItems;

namespace bom.Managers.BoughtOutItems.Abstractions
{
    public interface IBoughtOutItemMappingManager
    {
        Task<BoughtOutItemMapping> AddBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping);
        Task<BoughtOutItemMapping> GetBoughtOutItemMappingAsync(int id);
        Task<IEnumerable<BoughtOutItemMapping>> GetAllBoughtOutItemMappingsAsync();
        Task<bool> UpdateBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping);
        Task DeleteBoughtOutItemMappingAsync(int id);
    }
}
