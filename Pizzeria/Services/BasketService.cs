using System.Collections.Generic;
using Pizzeria.Data;
using Pizzeria.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Runtime.CompilerServices;
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
            var basketId = GetCurrentBasketId(contextSession);

            return _context.Baskets
                .Include(y => y.Items)
                .ThenInclude(x => x.Dish)
                .FirstOrDefault(x => x.BasketId == basketId);
        }

        private int GetCurrentBasketId(ISession contextSession)
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
                    var basketService = new BasketService(_context);
                    total += currentBasketItem.Quantity * basketService.GetPriceForBasketItem(currentBasketItem.BasketItemId);
                }

            return total;
        }

        public int GetPriceForBasketItem(int basketItemId)
        {
            var newItem = _context.BasketItems
                .Include(x => x.BasketItemIngredients)
                .ThenInclude(y => y.Ingredient)
                .FirstOrDefault(z => z.BasketItemId == basketItemId);

            var extraItemIngredientsIds = GetExtraItemIngredientIds(basketItemId, newItem.DishId);
           
            return newItem.Dish.Price + newItem.BasketItemIngredients.Where(bii => extraItemIngredientsIds.Any(id => id == bii.IngredientId)).Sum(bii => bii.Ingredient.Price);
        }

        private List<int> GetExtraItemIngredientIds(int basketItemId, int newItemDishId)
        {
            var originalIngredientIds = _context.DishIngredients.Where(di => di.DishId == newItemDishId)
                .Select(di => di.IngredientId).ToList();
            var basketItemIngredientIds = _context.BasketItemIngredients.Where(bii => bii.BasketItemId == basketItemId)
                .Select(bii => bii.IngredientId).ToList();

            return basketItemIngredientIds.Except(originalIngredientIds).ToList();
        }
    }       
}


