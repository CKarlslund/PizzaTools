using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class CreateDishViewModel
    {
        public Dish Dish { get; set; }
        public List<CheckBox> Ingredients { get; set; }
    }
}