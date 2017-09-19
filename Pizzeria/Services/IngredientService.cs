using System.Collections.Generic;
using System.Linq;
using Pizzeria.Data;
using Pizzeria.Models;

namespace Pizzeria.Services
{
    public class IngredientService
    {
        private readonly ApplicationDbContext _context;

        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Ingredient> AllForDish(int dishId)
        {
            var dishIngredients= _context.Dishes.FirstOrDefault(x => x.DishId == dishId).DishIngredients.Select(y => y.Ingredient).ToList();

            var ingredientsList = _context.Ingredients.OrderBy(x => x.Name).ToList();

            return ingredientsList;
        }

        public List<Ingredient> All()
        {
            return _context.Ingredients.OrderBy(x => x.Name).ToList();
        }
    }
}