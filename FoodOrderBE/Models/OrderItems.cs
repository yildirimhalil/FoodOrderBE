using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderBE.Models
{
    public class OrderItems
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int FoodID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
