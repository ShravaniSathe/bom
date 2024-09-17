using bom.Models.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.SubAssemblies;
using bom.Models.BoughtOutItems;
using bom.Models.BOMStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace bom.Models
{
    public class BOMContext : DbContext
    {
        public BOMContext(DbContextOptions<BOMContext> options) : base(options) { }

        public DbSet<ItemMasterSale> ItemMasterSales { get; set; }
        public DbSet<ItemMasterRawMaterial> ItemMasterRawMaterial { get; set; }
        public DbSet<SubAssemblie> SubAssemblies { get; set; }
        public DbSet<BoughtOutItem> BoughtOutItems { get; set; }
        public DbSet<BOMStructure> BOMStructures { get; set; }
        public DbSet<ItemRawMaterialMapping> ItemRawMaterialMappings { get; set; }
        public DbSet<RawMaterialBoughtOutMapping> RawMaterialBoughtOutMappings { get; set; }
    }
}

