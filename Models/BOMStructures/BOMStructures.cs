using bom.Models.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.BOMStructures
{
    public class BOMStructure : IEntity
    {
        public int BOMId { get; set; }
        public int ItemMasterSalesId { get; set; }
        public int ParentRawMaterialId { get; set; }
        public int ChildRawMaterialId { get; set; }
        public ItemMasterRawMaterial ItemMasterRawMaterial { get; set; }
        public ItemMasterSale ItemMasterSale { get; set; }
    }
}
