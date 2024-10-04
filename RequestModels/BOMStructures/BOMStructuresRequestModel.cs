using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;

namespace bom.API.RequestModels.BOMStructures
{
    public class BOMStructuresRequestModel
    {
        public int Id { get; set; }
        public int ItemMasterSalesId { get; set; }
        public int? ParentSubAssemblyId { get; set; }
        public int? ChildSubAssemblyId { get; set; }
        public int? ChildRawMaterialId { get; set; }
        public int Level { get; set; }
        public string PType { get; set; }
    }
}
