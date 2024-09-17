using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;

namespace bom.Repositories.ItemMasterSales.Abstractions
{
    public interface IItemMasterSalesRepository : IRepository<ItemMasterSale>
    {
        Task<ItemMasterSale> AddItemMasterSalesAsync(ItemMasterSale itemMasterSale);
        Task<ItemMasterSale> GetItemMasterSalesAsync(int id);
        Task<List<ItemMasterSale>> GetAllItemMasterSalesAsync();
        Task<ItemMasterSale> UpdateItemMasterSalesAsync(ItemMasterSale itemMasterSale);
        Task<bool> DeleteItemMasterSalesAsync(int id);

    }
}

