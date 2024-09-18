using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.BoughtOutItems;

namespace bom.Managers.BoughtOutItems.Abstractions
{
    public interface IBoughtOutItemsManager
    {
        Task<BoughtOutItem> AddBoughtOutItemAsync(BoughtOutItem boughtOutItem);
        Task<BoughtOutItem> GetBoughtOutItemAsync(int id);
        Task<IEnumerable<BoughtOutItem>> GetAllBoughtOutItemsAsync();
        Task<bool> UpdateBoughtOutItemAsync(BoughtOutItem boughtOutItem);
        Task DeleteBoughtOutItemAsync(int id);
    }
}
