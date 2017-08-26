using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;

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
            var thing = _context.Baskets.FirstOrDefault(x => x.BasketId == GetCurrentBasketId(contextSession));
            return thing;
        }

        private int GetCurrentBasketId(ISession contextSession)
        {
            var basketId = contextSession.GetInt32("BasketId");

            if (basketId.HasValue) return basketId.Value;
            var basket = new Basket(){Items = new List<BasketItem>()};
            _context.Baskets.Add(basket);
            _context.SaveChanges();
            contextSession.SetInt32("BasketId", basket.BasketId);
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


