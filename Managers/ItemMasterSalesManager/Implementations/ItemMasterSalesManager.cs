using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.API.RequestModels.ItemMasterSales;
using bom.Managers.ItemMasterSales.Abstractions;
using bom.Repositories.ItemMasterSales.Abstractions;

namespace bom.Managers.ItemMasterSales.Implementations
{
    public class ItemMasterSalesManager : IItemMasterSalesManager
    {
        private readonly IItemMasterSalesRepository _repo;

        public ItemMasterSalesManager(IItemMasterSalesRepository repo)
        {
            _repo = repo;
        }
        public async Task<ItemMasterSale> AddItemMasterSalesAsync(ItemMasterSale itemMasterSale)
        {
            return await _repo.AddItemMasterSalesAsync(itemMasterSale);
        }

        public async Task DeleteItemMasterSalesAsync(int id)
        {
            await _repo.DeleteItemMasterSalesAsync(id);
        }

        public async Task<List<ItemMasterSale>> GetAllItemMasterSalesAsync()
        {
            return await _repo.GetAllItemMasterSalesAsync();
        }

        public async Task<ItemMasterSale> GetItemMasterSalesAsync(int id)
        {
            return await _repo.GetItemMasterSalesAsync(id);
        }

        public async Task<ItemMasterSale> UpdateItemMasterSalesAsync(ItemMasterSale itemMasterSale)
        {
            return await _repo.UpdateItemMasterSalesAsync(itemMasterSale);
        }
    }
}
