using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pizzeria.Data;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DishController> _logger;

        public DishController(ApplicationDbContext context, ILogger<DishController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dishes.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogError("No dish ID");

                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .Include(b => b.Category)
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Name,Price,CategoryId,")] Dish dish, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                dish.ImageUrl = dish.Category.DefaultImage;

                var ingredientKeys = formCollection.Keys.Where(x => x.Contains("ingredient-"));

                dish.DishIngredients = new List<DishIngredient>();

                foreach (var ingredientKey in ingredientKeys)
                {
                    var splitKey = ingredientKey.Split("-");

                    var ingredientId = Convert.ToInt32(splitKey[1]);

                    dish.DishIngredients.Add(new DishIngredient() { IngredientId = ingredientId, Enabled = true, DishId = dish.DishId });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(x => x.DishIngredients).ThenInclude(y => y.Ingredient).SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,Name,Price,CategoryId")] Dish dish, IFormCollection formCollection)
        {
            if (id != dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dishIngredients = _context.DishIngredients.Where(x => x.DishId == dish.DishId).ToList();

                    if (dishIngredients.Count() != 0)
                    {
                        _context.DishIngredients.RemoveRange(dishIngredients);
                        await _context.SaveChangesAsync();
                    }
                                      
                    dish.ImageUrl = dish.ImageUrl;
                    _context.Update(dish);

                    var ingredientKeys = formCollection.Keys.Where(x => x.Contains("ingredient-"));

                    dish.DishIngredients = new List<DishIngredient>();

                    foreach (var ingredientKey in ingredientKeys)
                    {
                        var splitKey = ingredientKey.Split("-");

                        var ingredientId = Convert.ToInt32(splitKey[1]);

                        dish.DishIngredients.Add(new DishIngredient() { IngredientId = ingredientId, Enabled = true, DishId = dish.DishId });
                        }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
