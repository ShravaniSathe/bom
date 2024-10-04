namespace bom.API.RequestModels.BoughtOutItems
{
    public class BoughtOutItemsRequestModel
    {
        public int Id { get; set; }
        public int SubAssemblyId { get; set; } 
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public string PType { get; set; } = "BoughtOut"; 
    }
}
