using System.Collections.Generic;

namespace Pizzeria.Models.DishViewModels
{
    public class CreateDishViewModel
    {
        public Dish Dish { get; set; }
        public List<CheckBox> Ingredients { get; set; }
    }
}