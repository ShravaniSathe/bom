using System.ComponentModel.DataAnnotations;

namespace bom.API.RequestModels.ItemMasterRawMaterials
{
    public class RawMaterialBoughtOutMappingRequestModel
    {
        [Required]
        public int ItemMasterRawMaterialId { get; set; }

        [Required]
        public int BoughtOutId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost per unit must be greater than zero.")]
        public decimal CostPerUnit { get; set; }
    }
}
