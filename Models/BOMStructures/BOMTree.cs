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
            public int BOMId { get; set; }
            public string BOMName { get; set; }
            public List<BOMTreeNode> Nodes { get; set; }
        }

        public class BOMTreeNode : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? ParentId { get; set; } // Add this property for parent-child relationships
            public List<BOMTreeNode> Children { get; set; }
        }

 }
