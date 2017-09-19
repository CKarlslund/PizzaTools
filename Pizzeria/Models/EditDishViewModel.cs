using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class EditDishViewModel
    {
        public Dish Dish { get; set; }
        public List<CheckBox> Ingredients { get; set; }
    }

    public class CheckBox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}