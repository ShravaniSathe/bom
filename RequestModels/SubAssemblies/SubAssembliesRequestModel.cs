namespace bom.API.RequestModels.SubAssemblies
{
    public class SubAssembliesRequestModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public decimal CostPerUnit { get; set; }
    }
}
