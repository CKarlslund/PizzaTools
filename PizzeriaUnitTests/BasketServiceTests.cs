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
            var dish = new Dish()
            {
                DishId = 33
            };
            context.Dishes.Add(dish);

            var basketItem = new BasketItem()
            {
                Dish = dish,
                Quantity = 2,
                Price = 70
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
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
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
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetInt32("BasketId", 3);

            var basketService = new BasketService(_context);

            //Act
            var result = basketService.GetTotal(httpContext.Session);

            //Assert
            Assert.Equal(140, result);
        }
    }
}
