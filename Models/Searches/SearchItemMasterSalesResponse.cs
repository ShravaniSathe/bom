namespace bom.Models.Searches
{
    public class SearchItemMasterSalesResponse
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Grade { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public string PType { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
