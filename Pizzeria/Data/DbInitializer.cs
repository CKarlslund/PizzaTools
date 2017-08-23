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
                userManager.AddToRoleAsync(aUser, userRole.Name);
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
                userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }

            if (context.Dishes.ToList().Count == 0)
            {
                //Ingredients
                var cheese = new Ingredient() {Name = "Cheese", Description = "Cheesy cheese"};
                var tomato = new Ingredient() {Name = "Tomato", Description = "Tomato Tomahto"};

                //Categories
                var pizzaCategory = new Category() {Name = "Pizza", Description = "Wow pizza"};
                var pastaCategory = new Category() {Name = "Pasta", Description = "Wow pasta"};
                var saladCategory = new Category() {Name = "Salad", Description = "Wow salad"};
                var toolCategory = new Category() {Name = "Tools", Description = "Wow tools"};

                //Dishes
                var cappricciosa = new Dish(){Name = "Cappricciosa", Price = 70, Category = pizzaCategory };
                var margherita = new Dish() { Name = "marguerita", Price = 80, Category = pizzaCategory};
                var vegetariana = new Dish() { Name = "vegetariana", Price = 90, Category = pizzaCategory };
                var venuzio = new Dish() { Name = "Venuzio", Price = 100, Category = pizzaCategory };


                //DishIngredients
                var cappricciosacheese = new DishIngredient(){Dish = cappricciosa, Ingredient = cheese};
                var cappricciosatomato = new DishIngredient(){Dish = cappricciosa, Ingredient = tomato};

                cappricciosa.DishIngredients = new List<DishIngredient>
                {
                    cappricciosacheese,
                    cappricciosatomato
                };

                context.AddRange(cheese, tomato);
                context.AddRange(cappricciosatomato, cappricciosacheese);
                context.AddRange(pizzaCategory, pastaCategory, saladCategory, toolCategory);
                context.AddRange(cappricciosa, margherita, vegetariana, venuzio);
                context.SaveChanges();
            }
        }
    }
}

