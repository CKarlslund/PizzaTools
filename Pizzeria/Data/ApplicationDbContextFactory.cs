using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Pizzeria.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private IConfiguration _configuration;

        public ApplicationDbContextFactory()
        {
            //this._configuration = Startup.Configuration;
        }

        //public ApplicationDbContext Create(DbContextFactoryOptions options)
        //{
        //    var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        //    builder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        //    var dbContext = new ApplicationDbContext(builder.Options);
        //}

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=.\\SQLEXPRESS;database=Pizzeria;Trusted_Connection=True;MultipleActiveResultSets=true");
            var dbContext = new ApplicationDbContext(builder.Options);
            return dbContext;
        }
    }
}
