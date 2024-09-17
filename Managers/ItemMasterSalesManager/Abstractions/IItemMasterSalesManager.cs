using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;

namespace bom.Managers.ItemMasterSales.Abstractions
{
    public interface IItemMasterSalesManager
    {
        Task<ItemMasterSale> AddItemMasterSalesAsync(ItemMasterSale itemMasterSale);
        Task<ItemMasterSale> GetItemMasterSalesAsync(int id);
        Task<List<ItemMasterSale>> GetAllItemMasterSalesAsync();
        Task<ItemMasterSale> UpdateItemMasterSalesAsync(ItemMasterSale itemMasterSale);
        Task DeleteItemMasterSalesAsync(int id);

    }
}
