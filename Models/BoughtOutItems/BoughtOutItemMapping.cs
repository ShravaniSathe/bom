using bom.Models.ItemMasterSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace bom.Models.BoughtOutItems
{
    public class BoughtOutItemMapping : IEntity
    {
        public int Id { get; set; }
        public int BoughtOutItemId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public BoughtOutItem BoughtOutItem { get; set; }
        public ItemMasterSale ItemMasterSale { get; set; }
    }

}
