using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Data;

namespace Pizzeria.Commands
{
    public class UpdateQuantityHomeControllerCommand : BaseHomeControllerCommand
    {

        public async override Task<IActionResult> Execute(object id, IFormCollection formCollection)
        {
            return Controller.RedirectToAction("CustomizeIngredients", "Home", new {id, formCollection});
        }
    }
}
