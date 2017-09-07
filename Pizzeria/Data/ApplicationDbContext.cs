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

            builder.Entity<BasketItemIngredient>()
                .HasKey(ingredient => new { ingredient.BasketItemId, ingredient.IngredientId });

            builder.Entity<BasketItemIngredient>()
                .HasOne(dishIngredient => dishIngredient.BasketItem)
                .WithMany(dish => dish.BasketItemIngredients)
                .HasForeignKey(dishIngredient => dishIngredient.BasketItemId);

            builder.Entity<BasketItemIngredient>()
                .HasOne(basketItemIngredient => basketItemIngredient.Ingredient)
                .WithMany(ingredient => ingredient.BasketItemIngredients)
                .HasForeignKey(basketItemIngredient => basketItemIngredient.IngredientId);

            builder.Entity<Basket>()
                .HasOne(x => x.Order);

            builder.Entity<Order>()
                .HasOne(x => x.Basket);

            base.OnModelCreating(builder);
        }

        public virtual DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<BasketItemIngredient> BasketItemIngredients { get; set; }
        public DbSet<Pizzeria.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<CheckoutInfo> CheckoutInfo { get; set; }
    }
}
