using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Controllers;
using Pizzeria.Models;

namespace Pizzeria.Commands
{
    public class CustomizeHomeControllerCommand : BaseHomeControllerCommand 
    {
        public override async Task<IActionResult> Execute(object id, IFormCollection formCollection)
        {
            var context = this.Context;

            var basketItemId = Convert.ToInt32(id);
            var basketItem = context.BasketItems.FirstOrDefault(x => x.BasketItemId == basketItemId);

            var basketItemingredients = context.BasketItems
                .Include(y => y.BasketItemIngredients)
                .FirstOrDefault(x => x.BasketItemId == basketItemId)
                .BasketItemIngredients;

            context.BasketItemIngredients.RemoveRange(basketItemingredients);
            context.Update(basketItem);
            await context.SaveChangesAsync();

            var homeController = ((HomeController)this.Controller);

            var ingredientIds = homeController._ingredientService.All().Select(x => x.IngredientId);

            var basketItemIngredients = new List<BasketItemIngredient>();

            foreach (var ingredientId in ingredientIds)
            {
                if (formCollection.Keys.Any(k => k == $"ingredient_{ingredientId}"))
                {
                    var basketItemIngredient = new BasketItemIngredient()
                    {
                        Ingredient = context.Ingredients.FirstOrDefault(x => x.IngredientId == ingredientId),
                        Enabled = true
                    };
                    basketItemIngredients.Add(basketItemIngredient);
                }
            }
            context.BasketItems.FirstOrDefault(x => x.BasketItemId == basketItemId).BasketItemIngredients = basketItemIngredients;
            context.SaveChanges();

            return Controller.RedirectToAction("Index", "Home");
        }
    }
}
