using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
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
            var context = ServiceProvider.GetService<ApplicationDbContext>();

            CreateData(context);
        }

        private void CreateData(ApplicationDbContext context)
        {

            var ing1 = new Ingredient() { Name = "CCC" };
            var ing2 = new Ingredient() { Name = "BBB" };
            var ing3 = new Ingredient() { Name = "AAA" };

            context.AddRange(ing1,ing2,ing3);

            context.SaveChanges();

            var dish = new Dish()
            {
                DishIngredients = new List<DishIngredient>()
                {
                    
                }
            };
        }

        [Fact]
        public void All_Are_Sorted()
        {
            //Arrange
            var ingredientService = ServiceProvider.GetService<IngredientService>();

            //Act
            var ings = ingredientService.All();

            //Assert
            Assert.Equal(3, ings.Count);
            Assert.Equal("AAA", ings[0].Name );
            Assert.Equal("BBB", ings[1].Name);
            Assert.Equal("CCC", ings[2].Name );
        }

        [Fact]
        public void All_returns_correct_result()
        {
            //Arrange
            var ingredientService = ServiceProvider.GetService<IngredientService>();

            //Act
            var result = ingredientService.All();

            //Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void AllForDish_returns_correct_result()
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
