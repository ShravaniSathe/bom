using bom.Models.ItemMasterRawMaterials;
using bom.Models.ItemMasterSales;

namespace bom.API.RequestModels.ItemMasterSales
{
        public class ItemRawMaterialMappingRequestModel
        {
            public int Id { get; set; }
            public int ItemMasterSalesId { get; set; }
            public int ItemMasterRawMaterialId { get; set; }
            public decimal Quantity { get; set; }
            public string ProcureType { get; set; }
        }
    
}
