namespace bom.Models.Searches
{
    public class SearchBoughtOutItemsResponse
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int SubAssemblyId { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public string PType { get; set; } = "BoughtOut";
    }

}
