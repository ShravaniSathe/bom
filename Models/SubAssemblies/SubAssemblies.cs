using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bom.Models.SubAssemblies
{
    public class SubAssemblies : IEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public decimal CostPerUnit { get; set; }
    }
}
