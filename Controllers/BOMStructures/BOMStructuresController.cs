using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.Models.BOMStructures;
using bom.API.RequestModels.BOMStructures;
using bom.Managers.BOMStructures.Abstractions;

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BOMStructuresController : ControllerBase
    {
        private readonly IBOMStructuresManager _bomStructuresManager;

        public BOMStructuresController(IBOMStructuresManager bomStructuresManager)
        {
            _bomStructuresManager = bomStructuresManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create(BOMStructuresRequestModel bomStructureRequest)
        {
            try
            {
                var bomStructure = MapRequestObject(bomStructureRequest);
                var result = await _bomStructuresManager.AddBOMStructureAsync(bomStructure);
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
                var result = await _bomStructuresManager.GetBOMStructureAsync(id);
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
                var result = await _bomStructuresManager.GetAllBOMStructuresAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update(BOMStructuresRequestModel bomStructureRequest)
        {
            try
            {
                var bomStructure = MapRequestObject(bomStructureRequest);
                var result = await _bomStructuresManager.UpdateBOMStructureAsync(bomStructure);
                if (result != null)
                    return Ok("Update successful");
                else
                    return NotFound("BOM structure not found");
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
                await _bomStructuresManager.DeleteBOMStructureAsync(id);
                return Ok("BOM structure deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private BOMStructure MapRequestObject(BOMStructuresRequestModel bomStructureRequest)
        {
            var bomStructure = new BOMStructure
            {
                Id = bomStructureRequest.Id,
                ItemMasterSalesId = bomStructureRequest.ItemMasterSalesId,
                ParentSubAssemblyId = bomStructureRequest.ParentSubAssemblyId,
                ChildSubAssemblyId = bomStructureRequest.ChildSubAssemblyId,
                ChildRawMaterialId = bomStructureRequest.ChildRawMaterialId,
                Level = bomStructureRequest.Level,
                PType = bomStructureRequest.PType
            };

            return bomStructure;
        }
    }
}
