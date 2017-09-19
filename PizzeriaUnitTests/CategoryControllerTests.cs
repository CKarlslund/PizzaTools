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
    public class CategoryControllerTests : BasePizzeriaTests
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
            var categories = new List<Category>()
            {
                new Category() {CategoryId = 1, Name = "Pizza"},
                new Category() {CategoryId = 2, Name = "Pasta"},
                new Category() {CategoryId = 3, Name = "Salad"},
            };

            context.Categories.AddRange(categories);

            context.SaveChanges();
        }

        [Fact]
        public async Task Returns_correct_categories()
        {
            //Arrange         
            var mockLogger = new Mock<ILogger<CategoryController>>();

            var controller= new CategoryController(_context, mockLogger.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsCategoryDetail()
        {
            // Arrange
            var categoryId = 1;

            var mockLogger = new Mock<ILogger<CategoryController>>();

            var controller = new CategoryController(_context, mockLogger.Object);

            // Act
            var result = await controller.Details(categoryId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.NotNull(viewResult.ViewData);          
        }
    }
}
