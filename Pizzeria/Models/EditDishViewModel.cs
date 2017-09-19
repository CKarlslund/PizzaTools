using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class EditDishViewModel
    {
        public Dish Dish { get; set; }
        public List<CheckBox> Ingredients { get; set; }
    }
}