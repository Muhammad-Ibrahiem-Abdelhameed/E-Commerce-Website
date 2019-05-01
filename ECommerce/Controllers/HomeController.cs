using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(int Cate, string search)
        {
            ViewData["Categories"] = 
                new SelectList(_context.Categories, "Id", "Name");

            var products = from p in _context.Products.Include(p => p.Category)
                           select p;
            if (_signInManager.IsSignedIn(User))
            {
                var userId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;
                var cart = _context.ShoppingCarts.Where(sh => sh.UserId == userId).Count();
                ViewData["Cart"] = cart;

                TempData["UserId"] = userId;
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(s => s.Name.Contains(search));
            }

            /*if (!string.IsNullOrEmpty(Cate))
            {
                products = products.Where(x => x.CategoryId == Cate);
            }*/

            

            return View(await products.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> AddToShoppingCart(string id)
        {
            var userId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;

            var item = new ShoppingCart()
            {
                ProductId = id,
                UserId = userId,
                Quantity = 1

            };
            var checkItem = _context.ShoppingCarts
                .FirstOrDefault(s => s.UserId == userId && s.ProductId == id);
            
            if(checkItem != null)
            {
                checkItem.Quantity += 1;
                _context.Update(checkItem);                
            }
            else
            {
                _context.Add(item);
            }
            
            var result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                ViewData["Cart"] = Convert.ToInt32(ViewData["Cart"]) + 1;
                return Ok("Success");
            }
            return Ok("Fail");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
