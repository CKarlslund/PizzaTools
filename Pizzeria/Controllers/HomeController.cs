using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pizzeria.Commands;
using Pizzeria.Data;
using Pizzeria.Extensions;
using Pizzeria.Models;
using Pizzeria.Services;

namespace Pizzeria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public readonly IngredientService _ingredientService;

        public HomeController(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes
                .Include(b => b.Category)
                .Include(c => c.DishIngredients)
                .ThenInclude(e => e.Ingredient)
                .ToListAsync();

            ViewBag.HideSection = false;

            return View(dishes);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public Basket GetSessionObject(string sessionString)
        {
            return JsonConvert.DeserializeObject<Basket>(GetSessionString(sessionString));
        }
        
        public string GetSessionString(string sessionName)
        {
            return HttpContext.Session.GetString(sessionName);
        }

        public IActionResult FilterCategories(int id)
        {
            var products = _context.Dishes
                .Where(x => x.CategoryId.Equals(id))
                .Include(b => b.Category)
                .Include(c => c.DishIngredients)
                .ThenInclude(e => e.Ingredient)
                .ToList();

            return View("Index", products);
        }

        public async Task<IActionResult> ExecuteCommands(IFormCollection formCollection)
        {
            var result = formCollection.Keys.Select(x => new {tokens = x.Split("-")})
                .FirstOrDefault(y => y.tokens.Count() == 2);

            var key = formCollection.Keys.SingleOrDefault(x => x.Contains("-"));

            var splitKey = key.Split("-");

            var action = splitKey[0];

            string id = splitKey[1];

            var basketId = Convert.ToInt32(id);

            var commandName = $"Pizzeria.Commands.{action}HomeControllerCommand";

            BaseHomeControllerCommand cmd =
                (BaseHomeControllerCommand) Assembly.GetExecutingAssembly().CreateInstance(commandName);

            cmd.Context = _context;

            cmd.Controller = this;

            var actionResult = await cmd.Execute(basketId, formCollection);

            return actionResult;
        }

        public IActionResult CustomizePopup(int id)
        {
            var temp = _context.BasketItems
                .Include(x => x.BasketItemIngredients)
                .ThenInclude(y => y.Ingredient)
                .Include(z => z.Dish)
                .ToList();

            var basketItem = _context.BasketItems.Include(y => y.BasketItemIngredients).FirstOrDefault(x => x.BasketItemId == id);

            return PartialView("_CustomizePopup", basketItem);
        }

      [HttpPost]
        public IActionResult CustomizeIngredients(int basketId, IFormCollection formCollection)
        {
            return RedirectToAction("Index");
        }
    }
}
