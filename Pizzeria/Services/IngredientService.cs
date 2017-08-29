﻿using System.Collections.Generic;
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

        public List<Ingredient> All(int dishId)
        {
            var ingredients1= _context.Dishes.FirstOrDefault(x => x.DishId == dishId).DishIngredients.Select(y => y.Ingredient).ToList();

            var ingredientsList = _context.Ingredients.ToList();

            foreach (var ingredient in ingredientsList)
            {
                if (ingredients1.Exists(x => x.IngredientId == ingredient.IngredientId))
                {
                    ingredient.Enabled = true;
                }
            }

            return ingredientsList;
        }

        public List<Ingredient> All()
        {
            var temp = _context.Ingredients.ToList();

            return _context.Ingredients.ToList();
        }
    }
}