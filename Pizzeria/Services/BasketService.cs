using System.Collections.Generic;
using Pizzeria.Data;
using Pizzeria.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Pizzeria.Services
{
    public class BasketService 
    {
        private readonly ApplicationDbContext _context;

        public BasketService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Basket GetCurrentBasket(ISession contextSession)
        {
            var temp1 = GetCurrentBasketId(contextSession);

            var temp = _context.Baskets.Include(y => y.Items).FirstOrDefault(x => x.BasketId == temp1);
            return temp;
        }

        public int GetCurrentBasketId(ISession contextSession)
        {
            var basketId = contextSession.GetInt32("BasketId");

            if (basketId.HasValue) return basketId.Value;
            var basket = new Basket(){Items = new List<BasketItem>()};
            _context.Baskets.Add(basket);
            _context.SaveChanges();
            contextSession.SetInt32("BasketId", basket.BasketId);

            var temp = contextSession.GetInt32("BasketId");

            basketId = basket.BasketId;

            return basketId.Value;
        }

        public int GetTotal(ISession contextSession)
        {
            var currentBasket = GetCurrentBasket(contextSession);

            var total = 0;

            if (currentBasket?.Items != null)
                foreach (var currentBasketItem in currentBasket.Items)
                {
                    total += currentBasketItem.Quantity * currentBasketItem.Dish.Price;
                }

            return total;
        }
    }

       
}


