using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Models;

namespace Pizzeria.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> All()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
