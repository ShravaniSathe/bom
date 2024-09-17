using bom.Models.BoughtOutItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.ItemMasterRawMaterials
{
    public class RawMaterialBoughtOutMapping : IEntity
    {
        public int Id { get; set; }
        public int ItemMasterRawMaterialId { get; set; }
        public int BoughtOutId { get; set; }
        public decimal CostPerUnit { get; set; }
        public ItemMasterRawMaterial ItemMasterRawMaterial { get; set; }
        public BoughtOutItem BoughtOutItem { get; set; }
    }
}
