using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.SubAssemblies;
using bom.Models.Searches;
using bom.Managers.Search.Abstractions;
using bom.Repositories.Search.Abstractions;

namespace bom.Managers.Search.Implementations
{
    public class SearchManager : ISearchManager
    {
        private readonly ISearchRepository _searchRepository;

        public SearchManager(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<IEnumerable<SearchItemMasterSalesResponse>> SearchItemMasterSalesAsync(string query)
        {
            var itemMasterSales = await _searchRepository.SearchItemMasterSalesAsync(query);
            return itemMasterSales.Select(s => new SearchItemMasterSalesResponse
            {
                Id = s.Id,
                ItemName = s.ItemName,
                ItemCode = s.ItemCode,
                Grade = s.Grade,
                UOM = s.UOM,
                Quantity = s.Quantity,
                Level = s.Level,
                PType = s.PType,
                CreatedDate = s.CreatedDate
            });
        }

        public async Task<IEnumerable<SearchItemMasterRawMaterialsResponse>> SearchProductRawMaterialsAsync(string query)
        {
            var rawMaterials = await _searchRepository.SearchProductRawMaterialsAsync(query);
            return rawMaterials.Select(r => new SearchItemMasterRawMaterialsResponse
            {
                Id = r.Id,
                ItemName = r.ItemName,
                ItemCode = r.ItemCode,
                UOM = r.UOM,
                Quantity = r.Quantity,
                Grade = r.Grade
            });
        }

        public async Task<IEnumerable<SearchSubAssembliesResponse>> SearchSubAssembliesAsync(string query)
        {
            var subAssemblies = await _searchRepository.SearchSubAssembliesAsync(query);
            return subAssemblies.Select(s => new SearchSubAssembliesResponse
            {
                Id = s.Id,
                ItemName = s.ItemName,
                UOM = s.UOM,
                CostPerUnit = (decimal)s.CostPerUnit,
            });
        }

        
    }
}
