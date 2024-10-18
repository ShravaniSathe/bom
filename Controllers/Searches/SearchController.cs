using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using bom.Models.Searches;
using bom.Managers.Search.Abstractions; 

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchManager _searchManager;

        public SearchController(ISearchManager searchManager)
        {
            _searchManager = searchManager;
        }

        [HttpGet("item-master-sales")]
        public async Task<ActionResult> SearchItemMasterSales([FromQuery] SearchRequest request)
        {
            try
            {
                var results = await _searchManager.SearchItemMasterSalesAsync(request.Query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("product-raw-materials")]
        public async Task<ActionResult> SearchProductRawMaterials([FromQuery] SearchRequest request)
        {
            try
            {
                var results = await _searchManager.SearchProductRawMaterialsAsync(request.Query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sub-assemblies")]
        public async Task<ActionResult> SearchSubAssemblies([FromQuery] SearchRequest request)
        {
            try
            {
                var results = await _searchManager.SearchSubAssembliesAsync(request.Query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
    }
}
