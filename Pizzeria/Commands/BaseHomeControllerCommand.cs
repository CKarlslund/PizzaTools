using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Controllers;
using Pizzeria.Data;

namespace Pizzeria.Commands
{
    public abstract class BaseHomeControllerCommand
    {
        public ApplicationDbContext Context { get; set; }
        public HomeController Controller { get; set; }

        public abstract Task<IActionResult> Execute(object id);
    }
}