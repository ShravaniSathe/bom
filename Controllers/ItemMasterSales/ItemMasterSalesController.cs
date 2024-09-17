using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.API.RequestModels.ItemMasterSales;
using bom.Managers.ItemMasterSales.Abstractions;

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemMasterSalesController : ControllerBase
    {
        private readonly IItemMasterSalesManager _itemMasterSalesManager;

        public ItemMasterSalesController(IItemMasterSalesManager itemMasterSalesManager)
        {
            _itemMasterSalesManager = itemMasterSalesManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(ItemMasterSalesRequestModel itemMasterSalesRequest)
        {
            try
            {
                var itemMasterSales = MapRequestObject(itemMasterSalesRequest);
                var result = await _itemMasterSalesManager.AddItemMasterSalesAsync(itemMasterSales);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var result = await _itemMasterSalesManager.GetItemMasterSalesAsync(id);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = await _itemMasterSalesManager.GetAllItemMasterSalesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update(ItemMasterSalesRequestModel itemMasterSalesRequest)
        {
            try
            {
                ItemMasterSale itemMasterSales = MapRequestObject(itemMasterSalesRequest);
                var result = await _itemMasterSalesManager.UpdateItemMasterSalesAsync(itemMasterSales);
                if (result != null)
                    return Ok("Update successful");
                else
                    return NotFound("Item not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _itemMasterSalesManager.DeleteItemMasterSalesAsync(id);
                return Ok("Item deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ItemMasterSale MapRequestObject(ItemMasterSalesRequestModel itemMasterSalesRequest)
        {
            var itemMasterSale = new ItemMasterSale
            {
                Id = itemMasterSalesRequest.Id, 
                ItemName = itemMasterSalesRequest.ItemName,
                ItemCode = itemMasterSalesRequest.ItemCode,
                Grade = itemMasterSalesRequest.Grade,
                UOM = itemMasterSalesRequest.UOM,
                Quantity = itemMasterSalesRequest.Quantity,
                Level = itemMasterSalesRequest.Level,
                PType = itemMasterSalesRequest.PType,
                CreatedDate = itemMasterSalesRequest.CreatedDate 
            };

            return itemMasterSale;
        }
    }
}
