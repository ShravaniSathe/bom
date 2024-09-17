namespace bom.API.RequestModels.BOMStructures
{
    public class BOMTreeRequestModel
    {
        public int BOMId { get; set; }
        public string BOMName { get; set; }
        public List<BOMTreeNodeRequestModel> Nodes { get; set; }
    }

    public class BOMTreeNodeRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BOMTreeNodeRequestModel> Children { get; set; }
    }
}
