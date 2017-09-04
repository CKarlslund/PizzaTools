using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Extensions;
using Pizzeria.Models;

namespace Pizzeria.Commands
{
    public class AddHomeControllerCommand : BaseHomeControllerCommand
    {
        public override async Task<IActionResult> Execute(object id, IFormCollection formCollection)
        {
            var cartItemId = Convert.ToInt32(id);
            var dish = Context.Dishes
                .Include(y => y.DishIngredients)
                .ThenInclude(z => z.Ingredient)
                .FirstOrDefault(x => x.DishId == cartItemId);

            Basket basket;
            var session = Controller.HttpContext.Session;

            var basketId = session.GetInt32("BasketId");

            if (basketId == null)
            {
                basket = new Basket
                {
                    Items = new List<BasketItem>
                    {
                        new BasketItem()
                        {
                            Dish = dish,
                            Quantity = 1,
                        }
                    }
                };

                //Add basketItemIngredients
                List<BasketItemIngredient> basketItemIngredients =
                    dish.DishIngredients
                        .Select(x => new BasketItemIngredient() {Ingredient = x.Ingredient, Enabled = x.Enabled})
                        .ToList();

                basket.Items.First().BasketItemIngredients = basketItemIngredients;
            }
            else
            {
                basket = Context.Baskets.Include(y => y.Items)
                    .ThenInclude(g => g.BasketItemIngredients)
                    .ThenInclude(p => p.Ingredient)
                    .Include(l => l.Items)
                    .ThenInclude(h => h.Dish)
                    .SingleOrDefault(x => x.BasketId == basketId)
                         ?? new Basket();

                if (basket.Items != null && basket.Items.Exists(basketItem => basketItem.DishId == dish.DishId))
                {
                    var existingItem = basket.Items.FirstOrDefault(x => x.DishId == dish.DishId);
                    existingItem.Quantity++;
                }
                else
                {
                    List<BasketItemIngredient> basketItemIngredients = new List<BasketItemIngredient>();

                    foreach (var dishIngredient in dish.DishIngredients)
                    {
                        var newIngredient = new BasketItemIngredient()
                        {
                            Ingredient = dishIngredient.Ingredient,
                            Enabled = dishIngredient.Enabled
                        };
                        basketItemIngredients.Add(newIngredient);
                    }

                    basket.Items?.Add(new BasketItem()
                    {
                        Dish = dish,
                        Quantity = 1,
                        BasketItemIngredients = basketItemIngredients                        
                    });                    
                }
            }
            SaveBasket(basket);
            session.SetInt32("BasketId", basket.BasketId);

            var temp = session.GetInt32("BasketId");

           return Controller.RedirectToAction("Index");
        }

        private void SaveBasket(Basket basket)
        {
            Context.AddOrUpdate(basket);

            var temp = Context.Baskets.ToList();
            Context.SaveChanges();
        }
    }
}
