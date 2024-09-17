using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;

namespace bom.API.RequestModels.BOMStructures
{
    public class BOMStructuresRequestModel
    {
        public int Id { get; set; }
        public int ItemMasterSalesId { get; set; } // ID of the item master sale
        public int ParentRawMaterialId { get; set; } // ID of the parent raw material
        public int ChildRawMaterialId { get; set; } // ID of the child raw material
    }
}
