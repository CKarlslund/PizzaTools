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
            return _context.Baskets.FirstOrDefault(x => x.BasketId == GetCurrentBasketId(contextSession));
        }

        private int GetCurrentBasketId(ISession contextSession)
        {
            var basketId = contextSession.GetInt32("BasketId").Value;

            if (contextSession.GetInt32("BasketId").HasValue) return basketId;
            var basket = new Basket();
            _context.Baskets.Add(basket);
            _context.SaveChanges();
            contextSession.SetInt32("BasketId", basket.BasketId);
            basketId = basket.BasketId;

            return basketId;
        }

        public int GetTotal(ISession contextSession)
        {
            var currentBasket = GetCurrentBasket(contextSession).Items;

            var total = 0;

            if (currentBasket.Count > 0)
            {
                foreach (var currentBasketItem in currentBasket)
                {
                    total += currentBasketItem.Quantity * currentBasketItem.Dish.Price;
                }
            }

            return total;
        }
    }

       
}


