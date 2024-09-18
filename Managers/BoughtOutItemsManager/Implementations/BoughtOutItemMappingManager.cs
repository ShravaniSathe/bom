using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Managers.BoughtOutItems.Abstractions;
using bom.Repositories.BoughtOutItems.Abstractions;
using bom.Models.BoughtOutItems;

namespace bom.Managers.BoughtOutItems
{
    public class BoughtOutItemMappingManager : IBoughtOutItemMappingManager
    {
        private readonly IBoughtOutItemMappingRepository _boughtOutItemMappingRepository;
        private string connectionString;

        public BoughtOutItemMappingManager(IBoughtOutItemMappingRepository boughtOutItemMappingRepository)
        {
            _boughtOutItemMappingRepository = boughtOutItemMappingRepository;
        }

        public BoughtOutItemMappingManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<BoughtOutItemMapping> AddBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping)
        {
            return await _boughtOutItemMappingRepository.AddBoughtOutItemMappingAsync(boughtOutItemMapping);
        }

        public async Task<BoughtOutItemMapping> GetBoughtOutItemMappingAsync(int id)
        {
            return await _boughtOutItemMappingRepository.GetBoughtOutItemMappingAsync(id);
        }

        public async Task<IEnumerable<BoughtOutItemMapping>> GetAllBoughtOutItemMappingsAsync()
        {
            return await _boughtOutItemMappingRepository.GetAllBoughtOutItemMappingsAsync();
        }

        public async Task<BoughtOutItemMapping> UpdateBoughtOutItemMappingAsync(BoughtOutItemMapping boughtOutItemMapping)
        {
            return await _boughtOutItemMappingRepository.UpdateBoughtOutItemMappingAsync(boughtOutItemMapping);
        }

        public async Task DeleteBoughtOutItemMappingAsync(int id)
        {
            await _boughtOutItemMappingRepository.DeleteBoughtOutItemMappingAsync(id);
        }
    }
}
