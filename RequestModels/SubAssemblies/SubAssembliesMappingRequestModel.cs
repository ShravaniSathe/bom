namespace bom.API.RequestModels.SubAssemblies
{
    public class SubAssembliesMappingRequestModel
    {
       
            public int SubAssemblyId { get; set; }
            public int ItemId { get; set; }
            public int? RawMaterialId { get; set; }
            public decimal Quantity { get; set; }
        
    }

}

