using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemTests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Pizzeria.Controllers;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.Services;
using Xunit;

namespace PizzeriaUnitTests
{
    public class OrdersControllerTests : BasePizzeriaTests
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
            var user = new ApplicationUser(){FirstName = "August"};
            context.Users.Add(user);

            var temp = context.Users.FirstOrDefault(x => x.Id == user.Id);

            var basket = new Basket(){BasketId = 50};
            context.Baskets.Add(basket);
            var orders = new List<Order>()
            {
                new Order() {OrderId = 1, Basket = basket, User = user},
                new Order() {OrderId = 2, Basket = new Basket()},
                new Order() {OrderId = 3, Basket = new Basket()},
            };

            context.Order.AddRange(orders);           
            context.SaveChanges();
        }

        [Fact]
        public async Task Returns_correct_orders()
        {
            //Arrange         
            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockEmailSender = new Mock<IEmailSender>();

            var controller= new OrdersController(_context, mockLogger.Object, mockEmailSender.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Order>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsOrderDetail()
        {
            // Arrange
            var orderId = 1;

            var mockLogger = new Mock<ILogger<OrdersController>>();
            var mockEmailSender = new Mock<IEmailSender>();

            var controller = new OrdersController(_context, mockLogger.Object,mockEmailSender.Object);

            // Act
            var result = await controller.Details(orderId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            Assert.NotNull(viewResult.ViewData);
            var viewModel = Assert.IsType<Order>(viewResult.ViewData.Model);
            Assert.NotNull(viewModel.Basket);
            var basket = _context.Baskets.SingleOrDefault(g => g.BasketId == viewModel.BasketId);
            Assert.NotNull(basket);

            //Assert.NotNull(viewModel.User);
            //var user = _context.Users.SingleOrDefault(g => g.Id == viewModel.User.Id);
            //Assert.NotNull(user);
        }
    }
}
