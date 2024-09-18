using System.Collections.Generic;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.SubAssemblies;
using bom.Models.BoughtOutItems;

namespace bom.Repositories.Search.Abstractions
{
    public interface ISearchRepository
    {
        Task<IEnumerable<ItemMasterSale>> SearchItemMasterSalesAsync(string query);
        Task<IEnumerable<ItemMasterRawMaterial>> SearchProductRawMaterialsAsync(string query);
        Task<IEnumerable<SubAssemblie>> SearchSubAssembliesAsync(string query);
        Task<IEnumerable<BoughtOutItem>> SearchBoughtOutItemsAsync(string query);
    }
}
