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
    public class BoughtOutItemMappingController : ControllerBase
    {
        private readonly IBoughtOutItemMappingManager _boughtOutItemMappingManager;

        public BoughtOutItemMappingController(IBoughtOutItemMappingManager boughtOutItemMappingManager)
        {
            _boughtOutItemMappingManager = boughtOutItemMappingManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] BoughtOutItemMappingRequestModel request)
        {
            try
            {
                var mapping = new BoughtOutItemMapping
                {
                    BoughtOutItemId = request.BoughtOutItemId,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity
                };

                var result = await _boughtOutItemMappingManager.AddBoughtOutItemMappingAsync(mapping);
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
                var result = await _boughtOutItemMappingManager.GetBoughtOutItemMappingAsync(id);
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
                var result = await _boughtOutItemMappingManager.GetAllBoughtOutItemMappingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] BoughtOutItemMappingRequestModel request)
        {
            try
            {
                var mapping = new BoughtOutItemMapping
                {
                    BoughtOutItemId = request.BoughtOutItemId,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity
                };

                var result = await _boughtOutItemMappingManager.UpdateBoughtOutItemMappingAsync(mapping);
                if (result!=null)
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
        [Route("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _boughtOutItemMappingManager.DeleteBoughtOutItemMappingAsync(id);
                return Ok("Mapping deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
