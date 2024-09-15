﻿using bom.Models.ItemMasterSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.ItemMasterRawMaterials
{
    public class ItemMasterRawMaterial : IEntity
    {
        public int Id { get; set; }
        public ItemMasterSale ItemMasterSaleId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Grade { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public string PType { get; set; }
        public decimal CostPerUnit { get; set; }
    }
}
