using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.API.RequestModels.SubAssemblies;
using bom.Models.SubAssemblies;
using bom.Managers.SubAssemblies.Abstractions;

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAssemblyMappingController : ControllerBase
    {
        private readonly ISubAssemblyMappingManager _subAssemblyMappingManager;

        public SubAssemblyMappingController(ISubAssemblyMappingManager subAssemblyMappingManager)
        {
            _subAssemblyMappingManager = subAssemblyMappingManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(SubAssembliesMappingRequestModel request)
        {
            try
            {
                var subAssemblyMapping = MapRequestObject(request);
                var result = await _subAssemblyMappingManager.AddSubAssemblyMappingAsync(subAssemblyMapping);
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
                var result = await _subAssemblyMappingManager.GetSubAssemblyMappingAsync(id);
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
                var result = await _subAssemblyMappingManager.GetAllSubAssemblyMappingsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update(SubAssembliesMappingRequestModel request)
        {
            try
            {
                var subAssemblyMapping = MapRequestObject(request);
                var result = await _subAssemblyMappingManager.UpdateSubAssemblyMappingAsync(subAssemblyMapping);
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
        [Route("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _subAssemblyMappingManager.DeleteSubAssemblyMappingAsync(id);
                return Ok("Mapping deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private SubAssemblyMapping MapRequestObject(SubAssembliesMappingRequestModel request)
        {
            return new SubAssemblyMapping
            {
                SubAssemblyId = request.SubAssemblyId,
                ItemId = request.ItemId,
                RawMaterialId = request.RawMaterialId,
                Quantity = request.Quantity
            };
        }
    }
}
