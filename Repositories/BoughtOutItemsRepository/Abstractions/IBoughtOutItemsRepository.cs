using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.BoughtOutItems;

namespace bom.Repositories.BoughtOutItems.Abstractions
{
    public interface IBoughtOutItemsRepository : IRepository<BoughtOutItem>
    {
        Task<BoughtOutItem> AddBoughtOutItemAsync(BoughtOutItem boughtOutItem);
        Task<BoughtOutItem> GetBoughtOutItemAsync(int id);
        Task<IEnumerable<BoughtOutItem>> GetAllBoughtOutItemsAsync();
        Task<bool> UpdateBoughtOutItemAsync(BoughtOutItem boughtOutItem);
        Task DeleteBoughtOutItemAsync(int id);
    }
}
