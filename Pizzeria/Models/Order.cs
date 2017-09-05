using System;

namespace Pizzeria.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public bool Paid { get; set; } = false;
        public bool Delivered { get; set; } = false;
        public bool Finished { get; set; } = false;
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public int ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public Basket Basket { get; set; }
    }
}