using bom.Models.SubAssemblies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.BoughtOutItems
{
    public class BoughtOutItem : IEntity
    {
        public int Id { get; set; }
        public int SubAssemblyId { get; set; } 
        //public SubAssemblie SubAssembly { get; set; } 
        public string ItemName { get; set; } 
        public string UOM { get; set; } 
        public int Quantity { get; set; } 
        public decimal CostPerUnit { get; set; } 
        public string PType { get; set; } = "BoughtOut"; 
    }
}
