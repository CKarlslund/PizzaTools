using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public List<Category> All()
        {
            return _context.Categories.ToList();
        }

        public List<SelectListItem> GetSelectList(int dishId)
        {
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();

            var categoryId = _context.Dishes.Include(y => y.Category).FirstOrDefault(x => x.DishId == dishId).CategoryId;

            var list = new List<SelectListItem>();

            foreach (var category in categories)
            {
                bool selected = category.CategoryId == categoryId;

                var newItem = new SelectListItem()
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name,
                    Selected = selected
                };

                list.Add(newItem);
            }

            return list;
        }
    }
}
