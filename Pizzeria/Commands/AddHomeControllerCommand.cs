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
        public async override Task<IActionResult> Execute(object id)
        {
            var dish = Context.Dishes.FirstOrDefault(x => x.DishId == (int)id);

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
                            Quantity = 1
                        }
                    }
                };
            }
            else
            {
                basket = Context.Baskets.Include(y => y.Items).ThenInclude(h => h.Dish).SingleOrDefault(x => x.BasketId == basketId)
                         ?? new Basket();

                if (basket.Items != null && basket.Items.Exists(basketItem => basketItem.DishId == dish.DishId))
                {
                    var existingItem = basket.Items.FirstOrDefault(x => x.DishId == dish.DishId);
                    existingItem.Quantity++;
                }
                else
                {
                    basket.Items?.Add(new BasketItem()
                    {
                        Dish = dish,
                        Quantity = 1
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
