using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }
        public List<BasketItemIngredient> BasketItemIngredients { get; set; }
        public bool Enabled { get; set; }
    }
}