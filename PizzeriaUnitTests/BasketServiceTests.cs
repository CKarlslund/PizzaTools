using System.Collections.Generic;
using Pizzeria.Models;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Data;
using Xunit;

namespace PizzeriaUnitTests
{
    class BasketServiceTests : BasePizzeriaTests
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
                Items = new List<BasketItem>() { basketItem}
            };

            context.Baskets.Add(basket);

            context.SaveChanges();
        }

        [Fact]
        public void GetCurrentBasketId_returns_correct_basket()
        {
            //Arrange
            //var session =
            ////Act
            //var result = Pizzeria.Services.BasketService.GetCurrentBasketId(session);

            //Assert
        }

        [Fact]
        public void GetCurrentBasketId_returns_correct_id()
        {
            //Arrange
            //var session =
            ////Act
            //var result = Pizzeria.Services.BasketService.GetCurrentBasketId(session);

            //Assert
        }

        [Fact]
        public void GetTotal_returns_correct_total()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
