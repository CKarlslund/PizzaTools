using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Pizzeria.Models;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Data;
using Pizzeria.Services;
using Xunit;

namespace PizzeriaUnitTests
{
    public class BasketServiceTests : BasePizzeriaTests
    {
        private ApplicationDbContext _context;


        public override void InitializeDatabase()
        {
            base.InitializeDatabase();
            var context = ServiceProvider.GetService<ApplicationDbContext>();

            CreateData(context);
            _context = context;
        }

        private void CreateData(ApplicationDbContext context)
        {

            var ing1 = new Ingredient() {IngredientId = 1, Name = "Cheese", Price = 5};
            var ing2 = new Ingredient() {IngredientId = 2, Name = "Ham", Price = 10};
        
            context.Ingredients.AddRange(ing1,ing2);

            var dishIngredients = new List<DishIngredient>()
            {
                new DishIngredient(){Ingredient = ing1},
            };

            context.DishIngredients.AddRange(dishIngredients);

            var dish = new Dish()
            {
                Name = "Hans",
                Price = 50,
                DishId = 33,
                DishIngredients = dishIngredients
            };
            context.Dishes.Add(dish);

            var basketItemIngredients = new List<BasketItemIngredient>()
            {
                new BasketItemIngredient() {Ingredient = ing1},
                new BasketItemIngredient() {Ingredient = ing2}
            };
            context.BasketItemIngredients.AddRange(basketItemIngredients);

            var basketItem = new BasketItem()
            {
                BasketItemId = 45,
                Dish = dish,
                Quantity = 2,
                BasketItemIngredients = basketItemIngredients
            };
            context.BasketItems.Add(basketItem);

            var basket = new Basket()
            {
                BasketId = 3,
                Items = new List<BasketItem>() { basketItem}
            };

            context.Baskets.Add(basket);

            context.SaveChanges();
        }

        [Fact]
        public void GetCurrentBasket_returns_correct_basket()
        {
            //Arrange
            var httpContext = new DefaultHttpContext {Session = new TestSession()};
            httpContext.Session.SetInt32("BasketId", 3);

            var basketService = new BasketService(_context);

            //Act
            var result = basketService.GetCurrentBasket(httpContext.Session);

            //Assert
            Assert.Equal(3, result.BasketId);
        }
        
        [Fact]
        public void GetTotal_returns_correct_total()
        {
            //Arrange
            var httpContext = new DefaultHttpContext {Session = new TestSession()};
            httpContext.Session.SetInt32("BasketId", 3);

            var basketService = new BasketService(_context);

            //Act
            var result = basketService.GetTotal(httpContext.Session);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(120, result);
        }

        [Fact]
        public void GetPriceForBasketItem_returns_correct_total()
        {
            //Arrange

            var basketService = new BasketService(_context);

            //Act
            var result = basketService.GetPriceForBasketItem(45);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(60, result);
        }
    }
}
