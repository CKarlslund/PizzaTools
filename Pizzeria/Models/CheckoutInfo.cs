using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Pizzeria.Models
{
    public class CheckoutInfo
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Posting address")]
        public string PostingAddress { get; set; }

        [Display(Name = "Postal code")]
        public int PostalCode { get; set; }

        public string City { get; set; }

        [Display(Name = "Phone number")]
        public int PhoneNumber { get; set; }       
    }
}