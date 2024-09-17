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
        public int Id { get; set; }
        public ItemMasterSale ItemMasterSalesId { get; set; }
        public ItemMasterRawMaterial ParentRawMaterialId { get; set; }
        public ItemMasterRawMaterial ChildRawMaterialId { get; set; }
    }
}
