using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Pizzeria.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public override string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostingAddress { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }

        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
