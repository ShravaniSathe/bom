namespace bom.Models.Searches
{
    public class SearchBoughtOutItemsResponse
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Supplier { get; set; }
        public string UnitOfMeasure { get; set; }
        public int Quantity { get; set; }
    }

}
