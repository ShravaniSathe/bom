using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;
using bom.API.RequestModels.ItemMasterRawMaterials;
using bom.Managers.ItemMasterRawMaterials.Abstractions;

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemMasterRawMaterialsController : ControllerBase
    {
        private readonly IItemMasterRawMaterialsManager _itemMasterRawMaterialsManager;

        public ItemMasterRawMaterialsController(IItemMasterRawMaterialsManager itemMasterRawMaterialsManager)
        {
            _itemMasterRawMaterialsManager = itemMasterRawMaterialsManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(ItemMasterRawMaterialsRequestModel itemMasterRawMaterialsRequest)
        {
            try
            {
                var itemMasterRawMaterial = MapRequestObject(itemMasterRawMaterialsRequest);
                var result = await _itemMasterRawMaterialsManager.AddItemMasterRawMaterialAsync(itemMasterRawMaterial);
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
                var result = await _itemMasterRawMaterialsManager.GetItemMasterRawMaterialAsync(id);
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
                var result = await _itemMasterRawMaterialsManager.GetAllItemMasterRawMaterialsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update(ItemMasterRawMaterialsRequestModel itemMasterRawMaterialsRequest)
        {
            try
            {
                var itemMasterRawMaterial = MapRequestObject(itemMasterRawMaterialsRequest);
                var result = await _itemMasterRawMaterialsManager.UpdateItemMasterRawMaterialAsync(itemMasterRawMaterial);
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
                await _itemMasterRawMaterialsManager.DeleteItemMasterRawMaterialAsync(id);
                return Ok("Item deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ItemMasterRawMaterial MapRequestObject(ItemMasterRawMaterialsRequestModel itemMasterRawMaterialsRequest)
        {
            var itemMasterRawMaterial = new ItemMasterRawMaterial
            {
                Id = itemMasterRawMaterialsRequest.Id,
                ItemMasterSalesId = itemMasterRawMaterialsRequest.ItemMasterSalesId,
                ItemName = itemMasterRawMaterialsRequest.ItemName,
                ItemCode = itemMasterRawMaterialsRequest.ItemCode,
                Grade = itemMasterRawMaterialsRequest.Grade,
                UOM = itemMasterRawMaterialsRequest.UOM,
                Quantity = itemMasterRawMaterialsRequest.Quantity,
                Level = itemMasterRawMaterialsRequest.Level,
                PType = itemMasterRawMaterialsRequest.PType,
                CostPerUnit = itemMasterRawMaterialsRequest.CostPerUnit
            };

            return itemMasterRawMaterial;
        }
    }
}
