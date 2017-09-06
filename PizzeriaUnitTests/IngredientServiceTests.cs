using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.Services;
using Xunit;

namespace PizzeriaUnitTests
{
    public class IngredientServiceTests : BasePizzeriaTests
    {
        public override void InitializeDatabase()
        {
            base.InitializeDatabase();
            var context = _serviceProvider.GetService<ApplicationDbContext>();

            CreateData(context);
        }

        private void CreateData(ApplicationDbContext context)
        {
            context.Ingredients.Add(new Ingredient() { Name = "CCC" });
            context.Ingredients.Add(new Ingredient() { Name = "BBB" });
            context.Ingredients.Add(new Ingredient() { Name = "AAA" });

            context.SaveChanges();
        }

        [Fact]
        public void All_Are_Sorted()
        {
            //Arrange
            var ingredients = _serviceProvider.GetService<IngredientService>();

            //Act
            var ings = ingredients.All();

            //Assert
            Assert.Equal(3, ings.Count);
            Assert.Equal("AAA", ings[0].Name );
            Assert.Equal("BBB", ings[1].Name);
            Assert.Equal("CCC", ings[2].Name );
        }

    }
}
