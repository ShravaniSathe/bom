using bom.Models.ItemMasterSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace bom.Models.BOMStructure
 {
    public class BOMTree : IEntity
    {
        public int Id { get; set; }
        public int ItemMasterSalesId { get; set; }
        public ItemMasterSale ItemMasterSales { get; set; }
        public List<BOMTreeNode> Nodes { get; set; } = new List<BOMTreeNode>();
    }

    public class BOMTreeNode : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int Level { get; set; }
        public List<BOMTreeNode> Children { get; set; } = new List<BOMTreeNode>();
        public string NodeType { get; set; } 
        public string PType { get; set; }
    }


}
