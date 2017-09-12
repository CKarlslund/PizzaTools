using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Data;
using Pizzeria.Services;

namespace PizzeriaUnitTests
{
    public class BasePizzeriaTests
    {
        public readonly IServiceProvider ServiceProvider;

        public BasePizzeriaTests()
        {
            var efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b =>
                b.UseInMemoryDatabase("PizzaDatabase")
                    .UseInternalServiceProvider(efServiceProvider));

            services.AddTransient<BasketService>();
            services.AddTransient<IngredientService>();

            ServiceProvider = services.BuildServiceProvider();
            services.AddSingleton<IServiceProvider>(ServiceProvider);

            InitializeDatabase();
        }
        public virtual void InitializeDatabase()
        {
        }
    }
}
