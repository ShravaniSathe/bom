using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.Models.SubAssemblies;
using bom.API.RequestModels.SubAssemblies;
using bom.Managers.SubAssemblies.Abstractions;

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAssembliesController : ControllerBase
    {
        private readonly ISubAssembliesManager _subAssembliesManager;

        public SubAssembliesController(ISubAssembliesManager subAssembliesManager)
        {
            _subAssembliesManager = subAssembliesManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(SubAssembliesRequestModel subAssembliesRequest)
        {
            try
            {
                var subAssembly = MapRequestObject(subAssembliesRequest);
                var result = await _subAssembliesManager.AddSubAssemblyAsync(subAssembly);
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
                var result = await _subAssembliesManager.GetSubAssemblyAsync(id);
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
                var result = await _subAssembliesManager.GetAllSubAssembliesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update(SubAssembliesRequestModel subAssembliesRequest)
        {
            try
            {
                var subAssembly = MapRequestObject(subAssembliesRequest);
                var result = await _subAssembliesManager.UpdateSubAssemblyAsync(subAssembly);
                if (result != null)
                    return Ok("Update successful");
                else
                    return NotFound("Sub-assembly not found");
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
                await _subAssembliesManager.DeleteSubAssemblyAsync(id);
                return Ok("Sub-assembly deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private SubAssemblie MapRequestObject(SubAssembliesRequestModel subAssembliesRequest)
        {
            var subAssembly = new SubAssemblie
            {
                Id = subAssembliesRequest.Id,
                ItemName = subAssembliesRequest.ItemName,
                UOM = subAssembliesRequest.UOM,
                CostPerUnit = subAssembliesRequest.CostPerUnit
            };

            return subAssembly;
        }
    }
}
