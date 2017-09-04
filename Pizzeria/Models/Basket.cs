using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class Basket
    {
        public int BasketId { get; set; }

        public List<BasketItem> Items { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
