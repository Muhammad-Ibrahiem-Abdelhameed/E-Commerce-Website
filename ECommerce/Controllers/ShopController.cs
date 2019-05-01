using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;            
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        /*[Authorize]
        public async Task<IActionResult> AddToShoppingCart(string id)
        {
            var userId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;
            var product = _context.Users.FindAsync(User.Identity.Name);
            var item = new ShoppingCart()
            {
                ProductId = id,
                UserId = userId,
                Quantity = 1
               
            };
            _context.ShoppingCarts.Add(item);
            return View();
        }*/


        
    }
}