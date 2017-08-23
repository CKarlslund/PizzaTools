using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }
    }
}