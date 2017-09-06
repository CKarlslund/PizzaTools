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
        public readonly IServiceProvider _serviceProvider;

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

            _serviceProvider = services.BuildServiceProvider();
            services.AddSingleton<IServiceProvider>(_serviceProvider);

            InitializeDatabase();
        }
        public virtual void InitializeDatabase()
        {
        }
    }
}
