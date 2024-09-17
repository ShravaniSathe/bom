namespace bom.Models.Searches
{
    public class SearchProductRawMaterialsResponse
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string UnitOfMeasure { get; set; }
        public int Quantity { get; set; }
        public string Grade { get; set; }
    }

}
