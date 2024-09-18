using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Managers.BoughtOutItems.Abstractions;
using bom.Repositories.BoughtOutItems.Abstractions;
using bom.Models.BoughtOutItems;

namespace bom.Managers.BoughtOutItems.Implementations
{
    public class BoughtOutItemsManager : IBoughtOutItemsManager
    {
        private readonly IBoughtOutItemsRepository _boughtOutItemsRepository;

        public BoughtOutItemsManager(IBoughtOutItemsRepository boughtOutItemsRepository)
        {
            _boughtOutItemsRepository = boughtOutItemsRepository;
        }

        public async Task<BoughtOutItem> AddBoughtOutItemAsync(BoughtOutItem boughtOutItem)
        {
            return await _boughtOutItemsRepository.AddBoughtOutItemAsync(boughtOutItem);
        }

        public async Task<BoughtOutItem> GetBoughtOutItemAsync(int id)
        {
            return await _boughtOutItemsRepository.GetBoughtOutItemAsync(id);
        }

        public async Task<IEnumerable<BoughtOutItem>> GetAllBoughtOutItemsAsync()
        {
            return await _boughtOutItemsRepository.GetAllBoughtOutItemsAsync();
        }

        public async Task<bool> UpdateBoughtOutItemAsync(BoughtOutItem boughtOutItem)
        {
            return await _boughtOutItemsRepository.UpdateBoughtOutItemAsync(boughtOutItem);
        }

        public async Task DeleteBoughtOutItemAsync(int id)
        {
            await _boughtOutItemsRepository.DeleteBoughtOutItemAsync(id);
        }
    }
}
