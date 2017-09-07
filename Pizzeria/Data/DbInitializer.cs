using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Pizzeria.Models;
using Category = Pizzeria.Models.Category;

namespace Pizzeria.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser
            {
                UserName = "student@test.com",
                Email = "student@test.com"
            };

            var userResult = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            if (userResult.Succeeded)
            {
                var userRole = new IdentityRole() { Name = "User" };
                var roleResult = roleManager.CreateAsync(userRole).Result;
                var identityResult = userManager.AddToRoleAsync(aUser, userRole.Name).Result;
            }

            var adminUser = new ApplicationUser()
            {
                UserName = "admin@test.com",
                Email = "admin@mail.com"
            };

            var adminUserResult = userManager.CreateAsync(adminUser, "password").Result;

            if (adminUserResult.Succeeded)
            {
                var adminRole = new IdentityRole(){Name = "Admin"};
                var roleResult = roleManager.CreateAsync(adminRole).Result;
                var identityResult = userManager.AddToRoleAsync(adminUser, adminRole.Name).Result;
            }

            if (context.Dishes.ToList().Count == 0)
            {
                //Ingredients
                var cheese = new Ingredient() {Name = "Cheese", Description = "Cheesy cheese"};
                var tomato = new Ingredient() {Name = "Tomato", Description = "Tomato Tomahto"};
                var mushroom = new Ingredient() { Name = "Mushroom", Description = "Mushroom wow" };
                var gorgonzola = new Ingredient() { Name = "Gorgonzola", Description = "The best cheese. Better than everything." };

                //Categories
                var pizzaCategory = new Category() {Name = "Pizza", Description = "Wow pizza"};
                var pastaCategory = new Category() {Name = "Pasta", Description = "Wow pasta"};
                var saladCategory = new Category() {Name = "Salad", Description = "Wow salad"};
                var toolCategory = new Category() {Name = "Tools", Description = "Wow tools"};

                //Dishes
                var capricciosa = new Dish(){Name = "Capricciosa", Price = 70, Category = pizzaCategory, ImageUrl = "http://icons.iconarchive.com/icons/aha-soft/desktop-buffet/256/Pizza-icon.png" };
                var margherita = new Dish() { Name = "Margherita", Price = 80, Category = pizzaCategory, ImageUrl = "http://icons.iconarchive.com/icons/aha-soft/desktop-buffet/256/Pizza-icon.png" };
                var vegetariana = new Dish() { Name = "Vegetariana", Price = 90, Category = pizzaCategory, ImageUrl = "http://icons.iconarchive.com/icons/aha-soft/desktop-buffet/256/Pizza-icon.png" };
                var vesuvio = new Dish() { Name = "Vesuvio", Price = 100, Category = pizzaCategory, ImageUrl = "http://icons.iconarchive.com/icons/aha-soft/desktop-buffet/256/Pizza-icon.png" };

                var greekSalad = new Dish() { Name="Greek salad", Price=55, Category = saladCategory, ImageUrl = "http://icons.iconarchive.com/icons/aha-soft/desktop-buffet/256/Salad-icon.png" };
                var avocadoSalad = new Dish() { Name = "Avocado salad", Price = 50, Category = saladCategory, ImageUrl = "http://icons.iconarchive.com/icons/aha-soft/desktop-buffet/256/Salad-icon.png" };
                var eggSalad = new Dish() { Name = "Egg Salad", Price = 60, Category = saladCategory, ImageUrl = "http://icons.iconarchive.com/icons/aha-soft/desktop-buffet/256/Salad-icon.png" };

                //DishIngredients
                var cappricciosacheese = new DishIngredient(){Dish = capricciosa, Ingredient = cheese};
                var cappricciosatomato = new DishIngredient(){Dish = capricciosa, Ingredient = tomato};

                capricciosa.DishIngredients = new List<DishIngredient>
                {
                    cappricciosacheese,
                    cappricciosatomato
                };

                context.AddRange(cheese, tomato, mushroom, gorgonzola);
                context.AddRange(cappricciosatomato, cappricciosacheese);
                context.AddRange(pizzaCategory, pastaCategory, saladCategory, toolCategory);
                context.AddRange(capricciosa, margherita, vegetariana, vesuvio);
                context.AddRange(greekSalad, avocadoSalad, eggSalad);
                context.SaveChanges();
            }
        }
    }
}

