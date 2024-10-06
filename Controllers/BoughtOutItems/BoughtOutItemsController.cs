using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.Models.BoughtOutItems;
using bom.API.RequestModels.BoughtOutItems;
using bom.Managers.BoughtOutItems.Abstractions;

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoughtOutItemsController : ControllerBase
    {
        private readonly IBoughtOutItemsManager _boughtOutItemsManager;

        public BoughtOutItemsController(IBoughtOutItemsManager boughtOutItemsManager)
        {
            _boughtOutItemsManager = boughtOutItemsManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(BoughtOutItemsRequestModel boughtOutItemsRequest)
        {
            try
            {
                var boughtOutItem = MapRequestObject(boughtOutItemsRequest);
                var result = await _boughtOutItemsManager.AddBoughtOutItemAsync(boughtOutItem);
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
                var result = await _boughtOutItemsManager.GetBoughtOutItemAsync(id);
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
                var result = await _boughtOutItemsManager.GetAllBoughtOutItemsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update(BoughtOutItemsRequestModel boughtOutItemsRequest)
        {
            try
            {
                var boughtOutItem = MapRequestObject(boughtOutItemsRequest);
                var result = await _boughtOutItemsManager.UpdateBoughtOutItemAsync(boughtOutItem);
                if (result != null)
                    return Ok("Update successful");
                else
                    return NotFound("Bought-out item not found");
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
                await _boughtOutItemsManager.DeleteBoughtOutItemAsync(id);
                return Ok("Bought-out item deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private BoughtOutItem MapRequestObject(BoughtOutItemsRequestModel boughtOutItemsRequest)
        {
            var boughtOutItem = new BoughtOutItem
            {
                SubAssemblyId = boughtOutItemsRequest.SubAssemblyId,
                ItemName = boughtOutItemsRequest.ItemName,
                UOM = boughtOutItemsRequest.UOM,
                Quantity = boughtOutItemsRequest.Quantity,
                CostPerUnit = boughtOutItemsRequest.CostPerUnit,
                PType = boughtOutItemsRequest.PType 
            };

            return boughtOutItem;
        }
    }
}
