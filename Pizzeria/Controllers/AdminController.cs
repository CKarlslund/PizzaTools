using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController: Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
