using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Pizzeria.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultImage { get; set; }
    }
}