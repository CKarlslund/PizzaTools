using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;

namespace Pizzeria.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishIngredient>()
                .HasKey(dish => new {dish.DishId, dish.IngredientId});

            builder.Entity<DishIngredient>()
                .HasOne(dishIngredient => dishIngredient.Dish)
                .WithMany(dish => dish.DishIngredients)
                .HasForeignKey(dishIngredient => dishIngredient.DishId);

            builder.Entity<DishIngredient>()
                .HasOne(dishIngredient => dishIngredient.Ingredient)
                .WithMany(ingredient => ingredient.DishIngredients)
                .HasForeignKey(dishIngredient => dishIngredient.IngredientId);

            base.OnModelCreating(builder);
        }

        public virtual DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
    }
}
