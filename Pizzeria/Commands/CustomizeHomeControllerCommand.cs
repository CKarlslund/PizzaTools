using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pizzeria.Commands
{
    public class CustomizeHomeControllerCommand : BaseHomeControllerCommand 
    {
        public override Task<IActionResult> Execute(object id)
        {
            throw new NotImplementedException();
        }
    }
}
