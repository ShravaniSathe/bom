using Microsoft.EntityFrameworkCore;

namespace bom.Models
{
    public class BOMContext : DbContext
    {
        public BOMContext(DbContextOptions<BOMContext> options) : base(options) { }

        public DbSet<ItemMasterSales> ItemMasterSales { get; set; }
        public DbSet<ItemMasterRawMaterial> ItemMasterRawMaterial { get; set; }
        public DbSet<SubAssemblies> SubAssemblies { get; set; }
        public DbSet<BoughtOutItems> BoughtOutItems { get; set; }
        public DbSet<BOMStructure> BOMStructures { get; set; }
        public DbSet<ItemRawMaterialMapping> ItemRawMaterialMappings { get; set; }
        public DbSet<RawMaterialBoughtOutMapping> RawMaterialBoughtOutMappings { get; set; }
    }
}
