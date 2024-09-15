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
        public ItemMasterRawMaterial ItemMasterRawMaterialId { get; set; }
        public int BoughtOutId { get; set; }
        public decimal CostPerUnit { get; set; }
    }
}
