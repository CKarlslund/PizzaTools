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

namespace Pizzeria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
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

        //[HttpPost]
        //public IActionResult AddToBasket(int dishId)
        //{
        //    var dish = _context.Dishes.FirstOrDefault(x => x.DishId == dishId);

        //    Basket basket;

        //    var basketId = HttpContext.Session.GetInt32("BasketId");

        //    if (basketId == null)
        //    {
        //        basket = new Basket
        //        {
        //            Items = new List<BasketItem>
        //            {
        //                new BasketItem()
        //                {
        //                    Dish = dish,
        //                    Quantity = 1
        //                }
        //            }
        //        };
        //    }
        //    else
        //    {
        //        basket = _context.Baskets.Include(y => y.Items).SingleOrDefault(x => x.BasketId == basketId)
        //                 ?? new Basket();
                         
        //        if (basket.Items != null && basket.Items.Exists(basketItem => basketItem.DishId == dish.DishId))
        //        {
        //            var existingItem = basket.Items.FirstOrDefault(x => x.DishId == dish.DishId);
        //            existingItem.Quantity++;
        //        }
        //        else
        //        {
        //            basket.Items?.Add(new BasketItem()
        //            {
        //                Dish = dish,
        //                Quantity = 1
        //            });
        //        }
        //    }
        //    SaveBasket(basket);
        //    HttpContext.Session.SetInt32("BasketId", basket.BasketId);

        //    var what = HttpContext.Session.GetInt32("BasketId");

        //    return View("Index", _context.Dishes);
        //}

        public Basket GetSessionObject(string sessionString)
        {
            return JsonConvert.DeserializeObject<Basket>(GetSessionString(sessionString));
        }

        //private void SaveBasket(Basket basket)
        //{
        //    _context.AddOrUpdate(basket);

        //    var basketR = _context.Baskets.ToList();
        //    _context.SaveChanges();
        //}

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

            var action = result.tokens[0];

            var basketId = int.Parse(result.tokens[1]);

            var commandName = $"Pizzeria.Commands.{action}HomeControllerCommand";

            BaseHomeControllerCommand cmd =
                (BaseHomeControllerCommand) Assembly.GetExecutingAssembly().CreateInstance(commandName);

            cmd.Context = _context;

            cmd.Controller = this;

            var actionResult = await cmd.Execute(basketId);

            return actionResult;
        }
    }
}
