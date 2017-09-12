using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Pizzeria.Models;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Data;
using Xunit;

namespace PizzeriaUnitTests
{
    internal class BasketServiceTests : BasePizzeriaTests
    {
        public override void InitializeDatabase()
        {
            base.InitializeDatabase();
            var context = ServiceProvider.GetService<ApplicationDbContext>();

            CreateData(context);
        }

        private void CreateData(ApplicationDbContext context)
        {
            var dish = new Dish()
            {
                Price = 70
            };
            context.Dishes.Add(dish);

            var basketItem = new BasketItem()
            {
                Dish = dish,
                Quantity = 2
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
            ////Arrange
            //ISession session = new TestSession();
            //session.SetInt32("BasketId", 3);

            //////Act
            //var result = Pizzeria.Services.BasketService.GetCurrentBasket(session);

            ////Assert
            //Assert.Equal(result.BasketId, 3);
        }
        
        [Fact]
        public void GetTotal_returns_correct_total()
        {
            ////Arrange
            //ISession session = new TestSession();
            //session.SetInt32("BasketId", 3);

            ////Act
            //var result = Pizzeria.Services.BasketService.GetTotal(session);

            ////Assert
            //Assert.Equal(result, 140);
        }
    }
}
