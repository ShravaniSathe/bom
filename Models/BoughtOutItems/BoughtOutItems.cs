using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.BoughtOutItems
{
    public class BoughtOutItems : IEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Grade { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public int Level { get; set; }
        public decimal CostPerUnit { get; set; }
    }
}
