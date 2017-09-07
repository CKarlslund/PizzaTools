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

            var basket = context.Baskets
                .Include(y => y.Items)
                .ThenInclude(f => f.BasketItemIngredients)
                .FirstOrDefault(x => x.BasketId == Convert.ToInt32(id));

            //Create order
            var order = new Order
            {
                Basket = basket,
            };


            if (this.Controller.User.Identity.IsAuthenticated)
            {
                var userId = this.Controller.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = context.Users.FirstOrDefault(x => x.Id == userId);

                order.User = user;
                context.AddOrUpdate(order);
                await context.SaveChangesAsync();

                var checkoutInfo = new CheckoutInfo()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PostingAddress = user.PostingAddress
                    ,PostalCode = user.PostalCode,
                    City = user.City,
                    Email = user.Email,
                    PhoneNumber = Convert.ToInt32(user.PhoneNumber),
                    OrderId = order.OrderId
                };
                context.AddOrUpdate(checkoutInfo);

                await context.SaveChangesAsync();

                return Controller.RedirectToAction("Payment", "Orders", checkoutInfo);
            }

            return Controller.RedirectToAction("LoginOrAnonymous", "Orders");
        }
    }
}
