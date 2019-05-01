using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Controllers
{
    [Authorize]
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            
            string userId = "";
            if(TempData["UserId"] == null)
            {
                userId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;
            }
            else
            {
                userId = TempData["UserId"].ToString();
            }
            var applicationDbContext = _context.ShoppingCarts
                .Where(s => s.UserId == userId)
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product);

            ViewData["Cart"] = applicationDbContext.Count();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,UserId,Quantity")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.UserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.UserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ProductId,UserId,Quantity")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.UserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddToOrder()
        {
            string userId = "";
            if (TempData["UserId"] == null)
            {
                userId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;
            }
            else
            {
                userId = TempData["UserId"].ToString();
            }
            var shoppingCarts = await _context.ShoppingCarts
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .Where(s => s.UserId == userId)                
                .ToListAsync();

            var carts = shoppingCarts
                .Select(s => new CartHistory
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    QuantityPrice = s.QuantityPrice,
                })
                .ToList();

            decimal totalPrice = 0;
            foreach (var item in carts)
            {
                totalPrice += item.QuantityPrice;
            }
            var order = new Order { UserId = userId, IsShipped = false, TotalPrice = totalPrice };

            _context.Orders.Add(order);

            carts.ForEach(c => c.OrderId = order.Id);

            _context.CartHistories.AddRange(carts);

            _context.ShoppingCarts.RemoveRange(shoppingCarts);

            var result = await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Orders");

        }

        private bool ShoppingCartExists(string id)
        {
            return _context.ShoppingCarts.Any(e => e.Id == id);
        }
    }
}
