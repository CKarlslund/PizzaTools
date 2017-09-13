using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Controllers;
using Pizzeria.Extensions;
using Pizzeria.Models;

namespace Pizzeria.Commands
{
    public class CustomizeHomeControllerCommand : BaseHomeControllerCommand 
    {
        public override async Task<IActionResult> Execute(object id, IFormCollection formCollection)
        {
            var context = this.Context;

            var basketItemId = Convert.ToInt32(id);

            var oldBasketItem = context.BasketItems
                .Include(y => y.Dish)
                .Include(y => y.Basket)
                .FirstOrDefault(x => x.BasketItemId == basketItemId);

            var basket = context.Baskets.FirstOrDefault(x => x.BasketId == oldBasketItem.Basket.BasketId);

            //Get old basket item ingredients
            var oldBasketItemIngredients = context.BasketItems
                .Include(y => y.BasketItemIngredients)
                .Include(y => y.Basket)
                .FirstOrDefault(x => x.BasketItemId == basketItemId)
                .BasketItemIngredients;

            //Get new basket item ingredients
            var homeController = ((HomeController)this.Controller);

            var ingredientIds = homeController._ingredientService.All().Select(x => x.IngredientId);

            var newBasketItemIngredients = (from ingredientId in ingredientIds
                where formCollection.Keys.Any(k => k == $"ingredient_{ingredientId}")
                select new BasketItemIngredient()
                {
                    Ingredient = context.Ingredients.FirstOrDefault(x => x.IngredientId == ingredientId),
                    Enabled = true,
                    BasketItemId = oldBasketItem.BasketItemId,
                    IngredientId = ingredientId
                }).ToList();

            //Compare old and new
            var newBasketItemIngredientInts = newBasketItemIngredients.Select(x => x.IngredientId).ToList();
            var oldBasketItemIngredientInts = oldBasketItemIngredients.Select(y => y.IngredientId).ToList();
            
            var isEqual = newBasketItemIngredientInts.SequenceEqual(oldBasketItemIngredientInts);
           

            if (!isEqual)
            {
                var newBasketItem = new BasketItem()
                {
                    Name = oldBasketItem.Dish.Name + "-customized",
                    Dish = oldBasketItem.Dish,
                    Basket = basket,
                    Quantity = oldBasketItem.Quantity,
                    Price = oldBasketItem.Price,
                    BasketItemIngredients = newBasketItemIngredients,
                };

                var newIngredients = newBasketItemIngredientInts.Except(oldBasketItemIngredientInts).ToList();

                if (newIngredients.Count != 0)
                {
                    newBasketItem.Price = oldBasketItem.Dish.Price;

                    foreach (var newIngredient in newIngredients)
                    {
                        var ing = context.Ingredients.FirstOrDefault(x => x.IngredientId == newIngredient);

                        newBasketItem.Price += ing.Price;
                    }
                }

                context.BasketItems.Remove(oldBasketItem);
                context.SaveChanges();

                basket.Items.Remove(oldBasketItem);
                context.SaveChanges();

                basket.Items.Add(newBasketItem);
                context.SaveChanges();

                context.Update(oldBasketItem.Basket);
                context.SaveChanges();

                context.AddOrUpdate(newBasketItem);
                context.SaveChanges();
            }


            return Controller.RedirectToAction("Index", "Home");
        }
    }

    static class Extentions
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            var variances = new List<Variance>();
            var fi = val1.GetType().GetFields();
            foreach (FieldInfo f in fi)
            {
                Variance v = new Variance
                {
                    Prop = f.Name,
                    ValA = f.GetValue(val1),
                    ValB = f.GetValue(val2)
                };
                if (!Equals(v.ValA, v.ValB))
                        variances.Add(v);

            }
            return variances;
        }


    }
    class Variance
    {
        public string Prop { get; set; }
        public object ValA { get; set; }
        public object ValB { get; set; }
    }
}
