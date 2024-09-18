using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using bom.Models.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.SubAssemblies;
using bom.Models.BoughtOutItems;
using bom.Repositories.Search.Abstractions;
using System.Data.SqlClient;

namespace bom.Repositories.Search.Implementations
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IDbConnection db;

        public SearchRepository(string connectionString)
        {
            db = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<ItemMasterSale>> SearchItemMasterSalesAsync(string query)
        {
            const string storedProcedureName = "sp_SearchItemMasterSales";

            var result = await db.QueryAsync<ItemMasterSale>(storedProcedureName, new { Query = query }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<ItemMasterRawMaterial>> SearchProductRawMaterialsAsync(string query)
        {
            const string storedProcedureName = "sp_SearchProductRawMaterials";

            var result = await db.QueryAsync<ItemMasterRawMaterial>(storedProcedureName, new { Query = query }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<SubAssemblie>> SearchSubAssembliesAsync(string query)
        {
            const string storedProcedureName = "sp_SearchSubAssemblies";

            var result = await db.QueryAsync<SubAssemblie>(storedProcedureName, new { Query = query }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<BoughtOutItem>> SearchBoughtOutItemsAsync(string query)
        {
            const string storedProcedureName = "sp_SearchBoughtOutItems";

            var result = await db.QueryAsync<BoughtOutItem>(storedProcedureName, new { Query = query }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
