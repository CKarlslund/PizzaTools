using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Extensions;
using Pizzeria.Models;

namespace Pizzeria.Commands
{
    public class CheckoutHomeControllerCommand : BaseHomeControllerCommand
    {
        public override async Task<IActionResult> Execute(object id, IFormCollection formCollection)
        {
            var context = this.Context;

            var basketId = 0;

            if (this.Controller.HttpContext.Session.GetInt32("BasketId") != null)
            {
                basketId = this.Controller.HttpContext.Session.GetInt32("BasketId").Value;
            }

            var basket = context.Baskets
                .Include(x => x.Items)
                .ThenInclude(y => y.BasketItemIngredients)
                .Include(z => z.Items)
                .ThenInclude(h => h.Dish)
                .ThenInclude(j => j.DishIngredients)
                .ThenInclude(k => k.Ingredient)
                .FirstOrDefault(x => x.BasketId == basketId);

            var previousOrder = context.Order
                .Include(x => x.Basket)
                .FirstOrDefault(x => x.BasketId == basketId);

            var order = new Order();

            if (previousOrder != null)
            {
                order = previousOrder;
            }
            else
            {
                order.Basket = basket;
            }
            

            if (this.Controller.User.Identity.IsAuthenticated)
            {
                if (this.Controller.HttpContext.Session.GetInt32("LoggedInBefore") != null)
                {
                    order = context.Order
                        .Include(x => x.Basket)
                        .ThenInclude(y => y.Items)
                        .ToList().Last();
                }

                var userId = this.Controller.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = context.Users.FirstOrDefault(x => x.Id == userId);

                order.User = user;
                context.AddOrUpdate(order);

                await context.SaveChangesAsync();

                var chOutInfo = new CheckoutInfo
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PostingAddress = user.PostingAddress,
                    PostalCode = user.PostalCode,
                    City = user.City,
                    Email = user.Email,
                    PhoneNumber = Convert.ToInt32(user.PhoneNumber),
                    OrderId = order.OrderId
                };


                context.AddOrUpdate(chOutInfo);
                context.AddOrUpdate(order);
                order.BasketId = basketId;

                await context.SaveChangesAsync();

                this.Controller.HttpContext.Session.Remove("LoggedInBefore");

                return Controller.RedirectToAction("Payment", "Orders",chOutInfo);
            }

            context.AddOrUpdate(order);
            await context.SaveChangesAsync();

            var checkoutInfo = new CheckoutInfo { OrderId = order.OrderId };

            order.BasketId = basketId;
            await context.SaveChangesAsync();

            this.Controller.HttpContext.Session.SetInt32("LoggedInBefore", 1);

            return this.Controller.RedirectToAction("LoginOrAnonymous", "Orders", checkoutInfo);

            //return Controller.RedirectToAction("LoginOrAnonymous", "Orders");
        }
    }
}
