using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.API.RequestModels.ItemMasterRawMaterials;
using bom.Managers.ItemMasterRawMaterials.Abstractions;

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RawMaterialBoughtOutMappingController : ControllerBase
    {
        private readonly IRawMaterialBoughtOutMappingManager _rawMaterialBoughtOutMappingManager;

        public RawMaterialBoughtOutMappingController(IRawMaterialBoughtOutMappingManager rawMaterialBoughtOutMappingManager)
        {
            _rawMaterialBoughtOutMappingManager = rawMaterialBoughtOutMappingManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RawMaterialBoughtOutMappingRequestModel request)
        {
            try
            {
                var mapping = new RawMaterialBoughtOutMapping
                {
                    ItemMasterRawMaterialId = request.ItemMasterRawMaterialId,
                    BoughtOutId = request.BoughtOutId,
                    CostPerUnit = request.CostPerUnit
                };

                var result = await _rawMaterialBoughtOutMappingManager.AddRawMaterialBoughtOutMappingAsync(mapping);
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
                var result = await _rawMaterialBoughtOutMappingManager.GetRawMaterialBoughtOutMappingAsync(id);
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
                var result = await _rawMaterialBoughtOutMappingManager.GetAllRawMaterialBoughtOutMappingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] RawMaterialBoughtOutMappingRequestModel request)
        {
            try
            {
                var mapping = new RawMaterialBoughtOutMapping
                {
                    ItemMasterRawMaterialId = request.ItemMasterRawMaterialId,
                    BoughtOutId = request.BoughtOutId,
                    CostPerUnit = request.CostPerUnit
                };

                var result = await _rawMaterialBoughtOutMappingManager.UpdateRawMaterialBoughtOutMappingAsync(mapping);
                if (result!= null)
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
                await _rawMaterialBoughtOutMappingManager.DeleteRawMaterialBoughtOutMappingAsync(id);
                return Ok("Mapping deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
