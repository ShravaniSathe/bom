using bom.Models.BOMStructure;
using System.Collections.Generic;

namespace bom.API.RequestModels.BOMStructures
{
    public class BOMTreeRequestModel
    {
        public int Id { get; set; }
        public int ItemMasterSalesId { get; set; }
        public List<BOMTreeNodeRequestModel> Nodes { get; set; }
    }

    public class BOMTreeNodeRequestModel
    {
        public int Id { get; set; }
        public int BOMId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; } 
        public int Level { get; set; } 
        public string NodeType { get; set; } 
        public string PType { get; set; } 
        public List<BOMTreeNodeRequestModel> Children { get; set; }
    }
}
