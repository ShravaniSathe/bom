using bom.Models.ItemMasterRawMaterials;
using bom.Models.SubAssemblies;

namespace bom.API.RequestModels.SubAssemblies
{
    public class SubAssembliesRequestModel
    {
        public int Id { get; set; }
        public int ItemMasterSalesId { get; set; } 
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public decimal CostPerUnit { get; set; }
        public int Level { get; set; } 
        public int? ParentSubAssemblyId { get; set; } 
        public string PType { get; set; } 
        public List<ItemMasterRawMaterial> RawMaterials { get; set; } = new List<ItemMasterRawMaterial>(); 
        public List<SubAssemblie> ChildSubAssemblies { get; set; } = new List<SubAssemblie>(); 
    }
}
