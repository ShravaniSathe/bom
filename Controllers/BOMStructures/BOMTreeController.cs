using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using bom.API.RequestModels.BOMStructures;
using bom.Models.BOMStructure;
using bom.Managers.BOMStructures.Abstractions; 

namespace bom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BOMTreeController : ControllerBase
    {
        private readonly IBOMTreeManager _bomTreeManager; 

        public BOMTreeController(IBOMTreeManager bomTreeManager)
        {
            _bomTreeManager = bomTreeManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateBOMTree(BOMTreeRequestModel bomTreeRequest)
        {
            try
            {
                var bomTree = MapRequestObject(bomTreeRequest);

                var result = await _bomTreeManager.CreateBOMTreeAsync(bomTree);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{bomId}")]
        public async Task<ActionResult> GetBOMTree(int bomId)
        {
            try
            {
                var result = await _bomTreeManager.GetBOMTreeAsync(bomId);
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

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateBOMTree(BOMTreeRequestModel bomTreeRequest)
        {
            try
            {
                var bomTree = MapRequestObject(bomTreeRequest);

                var result = await _bomTreeManager.UpdateBOMTreeAsync(bomTree);

                if (result != null)
                    return Ok("Update successful");
                else
                    return NotFound("BOM tree not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{bomId}")]
        public async Task<ActionResult> DeleteBOMTree(int bomId)
        {
            try
            {
                await _bomTreeManager.DeleteBOMTreeAsync(bomId);
                return Ok("BOM tree deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private BOMTree MapRequestObject(BOMTreeRequestModel bomTreeRequest)
        {
            return new BOMTree
            {
                Id = bomTreeRequest.Id, 
                ItemMasterSalesId = bomTreeRequest.ItemMasterSalesId, 
                Nodes = MapNodes(bomTreeRequest.Nodes, null, 0) 
            };
        }

        private List<BOMTreeNode> MapNodes(List<BOMTreeNodeRequestModel> nodeRequests, int? parentId, int level)
        {
            var nodes = new List<BOMTreeNode>();

            foreach (var nodeRequest in nodeRequests)
            {
                var node = new BOMTreeNode
                {
                    Id = nodeRequest.Id,
                    BOMId = nodeRequest.BOMId,
                    Name = nodeRequest.Name,
                    ParentId = parentId,  
                    Level = level,
                    NodeType = nodeRequest.NodeType,
                    PType = nodeRequest.PType,
                    Children = MapNodes(nodeRequest.Children, nodeRequest.Id, level + 1)
                };

                nodes.Add(node);
            }

            return nodes;
        }

    }
}
