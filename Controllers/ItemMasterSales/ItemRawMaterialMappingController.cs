using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.API.RequestModels.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using bom.Managers.ItemRawMaterialMappings.Abstractions;
using bom.Models.ItemMasterSales; 

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemRawMaterialMappingsController : ControllerBase
    {
        private readonly IItemRawMaterialMappingManager _itemRawMaterialMappingManager; 

        public ItemRawMaterialMappingsController(IItemRawMaterialMappingManager itemRawMaterialMappingManager)
        {
            _itemRawMaterialMappingManager = itemRawMaterialMappingManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateItemRawMaterialMapping(ItemRawMaterialMappingRequestModel requestModel)
        {
            try
            {
                var itemRawMaterialMapping = MapRequestObject(requestModel);
                var result = await _itemRawMaterialMappingManager.AddItemRawMaterialMappingAsync(itemRawMaterialMapping);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetItemRawMaterialMapping(int id)
        {
            try
            {
                var result = await _itemRawMaterialMappingManager.GetItemRawMaterialMappingAsync(id);
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
        public async Task<ActionResult> GetAllItemRawMaterialMappings()
        {
            try
            {
                var result = await _itemRawMaterialMappingManager.GetAllItemRawMaterialMappingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateItemRawMaterialMapping(ItemRawMaterialMappingRequestModel requestModel)
        {
            try
            {
                var itemRawMaterialMapping = MapRequestObject(requestModel);
                var result = await _itemRawMaterialMappingManager.UpdateItemRawMaterialMappingAsync(itemRawMaterialMapping);
                if (result != null)
                    return Ok("Update successful");
                else
                    return NotFound("Mapping not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteItemRawMaterialMapping(int id)
        {
            try
            {
                await _itemRawMaterialMappingManager.DeleteItemRawMaterialMappingAsync(id);
                return Ok("Mapping deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ItemRawMaterialMapping MapRequestObject(ItemRawMaterialMappingRequestModel requestModel)
        {
            return new ItemRawMaterialMapping
            {
                Id = requestModel.Id,
                ItemMasterSalesId = requestModel.ItemMasterSalesId, 
                ItemMasterRawMaterialId = requestModel.ItemMasterRawMaterialId, 
                Quantity = requestModel.Quantity,
                ProcureType = requestModel.ProcureType
            };
        }

    }
}
