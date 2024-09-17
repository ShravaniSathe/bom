using System.ComponentModel.DataAnnotations;

namespace bom.API.RequestModels.BoughtOutItems
{
    public class BoughtOutItemMappingRequestModel
    {
        [Required]
        public int BoughtOutItemId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public decimal Quantity { get; set; }
    }
}
