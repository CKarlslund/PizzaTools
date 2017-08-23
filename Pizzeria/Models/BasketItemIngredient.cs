namespace Pizzeria.Models
{
    public class BasketItemIngredient
    {
        public int BasketItemId{ get; set; }
        public BasketItem BasketItem { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public bool Enabled { get; set; }
    }
}