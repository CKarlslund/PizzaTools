using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class BasketItem
    {
        public int BasketItemId { get; set; }

        public string Name { get; set; }
        public Basket Basket { get; set; }

        public int Quantity { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }

        public List<BasketItemIngredient> BasketItemIngredients { get; set; }
    }
}
