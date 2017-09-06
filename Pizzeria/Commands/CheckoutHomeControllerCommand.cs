using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;

namespace Pizzeria.Commands
{
    public class CheckoutHomeControllerCommand : BaseHomeControllerCommand
    {
        public override async Task<IActionResult> Execute(object id, IFormCollection formCollection)
        {
            var context = this.Context;

            //Handle quantity updates
            var keys = formCollection["item.Quantity"];
            var basket = context.Baskets
                .Include(y => y.Items)
                .ThenInclude(f => f.BasketItemIngredients)
                .FirstOrDefault(x => x.BasketId == Convert.ToInt32(id));

            for (var i = 0; i < basket.Items.Count; i++)
            {
                basket.Items[i].Quantity = Convert.ToInt32(keys[i]);
            }
            await context.SaveChangesAsync();

            //Create order
            var order = new Order();

            order.Basket = basket;
            order.User = new ApplicationUser();

            if (this.Controller.User.Identity.IsAuthenticated)
            {
                var userId = this.Controller.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = context.Users.FirstOrDefault(x => x.Id == userId);

                user.Orders.Add(order);

                await context.SaveChangesAsync();

                return Controller.RedirectToAction("Payment", "Orders", user);
            }

            return Controller.RedirectToAction("LoginOrAnonymous", "Orders");
        }
    }
}
