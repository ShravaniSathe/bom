using bom.Models.ItemMasterSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.BOMStructure
{
    public class BOMTree
    {
        public int BOMId { get; set; }
        public string BOMName { get; set; }
        public List<BOMTreeNode> Nodes { get; set; } 
    }

    public class BOMTreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BOMTreeNode> Children { get; set; } 
    }

}
