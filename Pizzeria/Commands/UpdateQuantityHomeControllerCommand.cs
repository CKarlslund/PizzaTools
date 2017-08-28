using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pizzeria.Commands
{
    public class UpdateQuantityHomeControllerCommand : BaseHomeControllerCommand
    {
        public async override Task<IActionResult> Execute(object id)
        {
            return Controller.RedirectToAction("Index");
        }
    }
}
