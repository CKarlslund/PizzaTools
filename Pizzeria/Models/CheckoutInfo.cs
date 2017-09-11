using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.AccessControl;

namespace Pizzeria.Models
{
    public class CheckoutInfo
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Posting address")]
        public string PostingAddress { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid postal code")]
        [Display(Name = "Postal code")]
        public int PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid phone number")]
        [Display(Name = "Phone number")]
        public int PhoneNumber { get; set; }       

        public string PaymentAuthorizationCode { get; set; }
    }
}