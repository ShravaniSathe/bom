using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;

namespace bom.Models.SubAssemblies
{
    public class SubAssemblie : IEntity
    {
        public int Id { get; set; }
        public int ItemMasterSalesId { get; set; } 
        //public ItemMasterSale ItemMasterSales { get; set; } 
        public string ItemName { get; set; } 
        public string UOM { get; set; } 
        public decimal? CostPerUnit { get; set; } 
        public int Level { get; set; } 
        public int? ParentSubAssemblyId { get; set; } 
        public string PType { get; set; }
        public List<ItemMasterRawMaterial> RawMaterials { get; set; } = new List<ItemMasterRawMaterial>();
        public List<SubAssemblie> ChildSubAssemblies { get; set; } = new List<SubAssemblie>();
        public dynamic Quantity { get; internal set; }
    }

}

