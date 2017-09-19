using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.Models.DishViewModels;
using Pizzeria.Services;

namespace Pizzeria.Controllers
{
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DishController> _logger;
        private IngredientService _ingredientService;
        private DishService _dishService;

        public DishController(ApplicationDbContext context, ILogger<DishController> logger, IngredientService ingredientService, DishService dishService)
        {
            _context = context;
            _logger = logger;
            _ingredientService = ingredientService;
            _dishService = dishService;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new CreateDishViewModel
            {
                Dish = new Dish(),
                Ingredients = new List<CheckBox>()
            };

            foreach (var ingredient in _ingredientService.All())
            {
                model.Ingredients.Add(new CheckBox
                {
                    Id = ingredient.IngredientId,
                    Name = ingredient.Name,
                    Selected = false
                });
            }

            return View(model);
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDishViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _context.Categories.FirstOrDefault(x => x.CategoryId == model.Dish.CategoryId);

                
                _context.Dishes.Add(model.Dish);
                _context.SaveChanges();

                model.Dish.ImageUrl = category.DefaultImage;

                foreach (var ingredient in model.Ingredients.Where(i => i.Selected))
                {
                    _context.DishIngredients.Add(new DishIngredient
                    {
                        DishId = model.Dish.DishId,
                        IngredientId = ingredient.Id
                    });                    
                }
                _context.Update(model.Dish);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Dishes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var dish = await _context.Dishes.Include(x => x.DishIngredients).ThenInclude(y => y.Ingredient).SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            var model = new EditDishViewModel
            {
                Dish = dish,
                Ingredients = new List<CheckBox>()
            };

            foreach (var ingredient in _ingredientService.All())
                {
                    model.Ingredients.Add(new CheckBox
                    {
                        Id = ingredient.IngredientId,
                        Name = ingredient.Name,
                        Selected = _dishService.HasIngredient(dish.DishId, ingredient.IngredientId)
                    });
                }
                
            return View(model);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDishViewModel model)
        {
            if (id != model.Dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.DishIngredients
                        .Where(i => i.DishId == model.Dish.DishId)
                        .ForEachAsync(di => _context.Remove(di));

                    await _context.SaveChangesAsync();

                    foreach (var ingredient in model.Ingredients.Where(i => i.Selected))
                    {
                        _context.DishIngredients.Add(new DishIngredient
                        {
                            DishId = id,
                            IngredientId = ingredient.Id
                         });

                        _context.Update(model.Dish);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException exc)
                {
                    if (!DishExists(model.Dish.DishId))
                    {
                        _logger.LogError(exc.ToString());
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Dishes/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
