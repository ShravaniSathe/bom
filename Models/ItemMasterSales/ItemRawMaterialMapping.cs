using bom.Models.ItemMasterRawMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.ItemMasterSales
{
    public class ItemRawMaterialMapping : IEntity
    {
        public int Id { get; set; }
        public ItemMasterSale ItemMasterSalesId { get; set; }
        public ItemMasterRawMaterial ItemMasterRawMaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string ProcureType { get; set; }
    }
}
