﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bom.Models.ItemMasterSales;
using bom.Models.ItemMasterRawMaterials;
using bom.Models.SubAssemblies;

namespace bom.Models.SubAssemblies
{
    public class SubAssemblyMapping : IEntity
    {
        public int Id { get; set; }
        public int SubAssemblyId { get; set; }
        public int ItemId { get; set; }
        public int? RawMaterialId { get; set; } 
        public decimal Quantity { get; set; }

    }

}
