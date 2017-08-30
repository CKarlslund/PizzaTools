using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pizzeria.Commands
{
    public class CustomizeHomeControllerCommand : BaseHomeControllerCommand 
    {
        public override async Task<IActionResult> Execute(object id)
        {
            var basketItemId = Convert.ToInt32(id);

            return Controller.RedirectToAction("Customize", "Home", new {BasketItemId = basketItemId });
        }
    }
}
