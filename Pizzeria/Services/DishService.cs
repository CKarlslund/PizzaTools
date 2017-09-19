using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Models;

namespace Pizzeria.Services
{
    public class DishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public List<Ingredient> AllForDish(int dishId)
        //{
        //    var dishIngredients = _context.Dishes.FirstOrDefault(x => x.DishId == dishId).DishIngredients.Select(y => y.Ingredient).ToList();

        //    var ingredientsList = _context.Ingredients.OrderBy(x => x.Name).ToList();

        //    foreach (var ingredient in ingredientsList)
        //    {
        //        if (dishIngredients.Exists(x => x.IngredientId == ingredient.IngredientId))
        //        {
        //            ingredient.Enabled = true;
        //        }
        //    }

        //    return ingredientsList;
        //}

        public List<Dish> All()
        {
            return _context.Dishes.OrderBy(x => x.Name).ToList();
        }

        public bool HasIngredient(int dishId, int ingredientId)
        {
            var dish = _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .FirstOrDefault(x => x.DishId == dishId);

            var hasIngredient = dish.DishIngredients.Exists(di => di.IngredientId == ingredientId);

            return hasIngredient;
        }
    }
}