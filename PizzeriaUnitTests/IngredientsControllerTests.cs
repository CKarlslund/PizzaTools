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
using Xunit;

namespace PizzeriaUnitTests
{
    public class IngredientsControllerTests : BasePizzeriaTests
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
            var ingredients = new List<Ingredient>()
            {
                new Ingredient() {IngredientId = 1, Name = "Cheese", },
                new Ingredient() {IngredientId = 2, Name = "Ham"},
                new Ingredient() {IngredientId = 3, Name = "Tomato"},
            };

            context.Ingredients.AddRange(ingredients);

            context.SaveChanges();
        }

        [Fact]
        public async Task Returns_correct_ingredients()
        {
            //Arrange         
            var mockLogger = new Mock<ILogger<IngredientsController>>();

            var controller= new IngredientsController(_context, mockLogger.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Ingredient>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsIngredientDetail()
        {
            // Arrange
            var ingredientId = 1;

            var mockLogger = new Mock<ILogger<IngredientsController>>();

            var controller = new IngredientsController(_context, mockLogger.Object);

            // Act
            var result = await controller.Details(ingredientId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);
        }
    }
}
