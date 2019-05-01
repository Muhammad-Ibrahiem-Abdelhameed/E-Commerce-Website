using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<CartHistory> CartHistories { get; set; }
    }
}
