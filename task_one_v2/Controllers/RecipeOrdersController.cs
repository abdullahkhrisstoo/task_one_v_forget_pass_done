using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task_one_v2.Models;

namespace task_one_v2.Controllers
{
    public class RecipeOrdersController : Controller
    {
        private readonly ModelContext _context;

        public RecipeOrdersController(ModelContext context)
        {
            _context = context;
        }

       


        public IActionResult Create()
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId");
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Userid", "Userid");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserId,RecipeId,OrderDate,TotalPrice")] RecipeOrder recipeOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction("AllOrder","Admin");
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", recipeOrder.RecipeId);
            ViewData["UserId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipeOrder.UserId);
            return View(recipeOrder);
        }

        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.RecipeOrders == null)
            {
                return NotFound();
            }

            var recipeOrder = await _context.RecipeOrders
                .Include(r => r.Recipe)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (recipeOrder == null)
            {
                return NotFound();
            }

            return View(recipeOrder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.RecipeOrders == null)
            {
                return Problem("Entity set 'ModelContext.RecipeOrders'  is null.");
            }
            var recipeOrder = await _context.RecipeOrders.FindAsync(id);
            if (recipeOrder != null)
            {
                _context.RecipeOrders.Remove(recipeOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("AllOrder", "Admin");
        }

        private bool RecipeOrderExists(decimal id)
        {
          return (_context.RecipeOrders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
