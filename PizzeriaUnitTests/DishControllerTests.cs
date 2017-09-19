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
    public class DishControllerTests : BasePizzeriaTests
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
            var dishes = new List<Dish>()
            {
                new Dish() {DishId = 1, Name = "Hawaii", CategoryId = 1},
                new Dish() {DishId = 2, Name = "Vezuvio"},
                new Dish() {DishId = 3, Name = "Vegetariana"},
            };

            context.Dishes.AddRange(dishes);

            var category = new Category()
            {
                CategoryId = 1
            };

            context.Categories.Add(category);

            context.SaveChanges();
        }

        [Fact]
        public async Task Returns_correct_dishes()
        {
            //Arrange         
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockIngredientService = new Mock<IngredientService>();
            var mockDishService = new Mock<DishService>();


            var controller= new DishController(_context, mockLogger.Object, mockIngredientService.Object, mockDishService.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Dish>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsDishDetail()
        {
            // Arrange
            var dishId = 1;

            var mockLogger = new Mock<ILogger<DishController>>();
            var mockIngredientService = new Mock<IngredientService>();
            var mockDishService = new Mock<DishService>();

            var controller = new DishController(_context, mockLogger.Object, mockIngredientService.Object, mockDishService.Object);

            // Act
            var result = await controller.Details(dishId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            Assert.NotNull(viewResult.ViewData);
            var viewModel = Assert.IsType<Dish>(viewResult.ViewData.Model);
            Assert.NotNull(viewModel.Category);
            var category = _context.Categories.SingleOrDefault(g => g.CategoryId == viewModel.CategoryId);
            Assert.NotNull(category);
        }
    }
}
