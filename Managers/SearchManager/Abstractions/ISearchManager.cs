using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.SubAssemblies;
using bom.Models.BoughtOutItems;
using bom.Models.Searches;

namespace bom.Managers.Search.Abstractions
{
    public interface ISearchManager
    {
        Task<IEnumerable<SearchItemMasterSalesResponse>> SearchItemMasterSalesAsync(string query);
        Task<IEnumerable<SearchItemMasterRawMaterialsResponse>> SearchProductRawMaterialsAsync(string query);
        Task<IEnumerable<SearchSubAssembliesResponse>> SearchSubAssembliesAsync(string query);
        Task<IEnumerable<SearchBoughtOutItemsResponse>> SearchBoughtOutItemsAsync(string query);
    }
}
