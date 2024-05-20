using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task_one_v2.Models;
using task_one_v2.App_Core.FileHelper;

namespace task_one_v2.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly FileHelper _fileHelper;

        public RecipesController(ModelContext context, FileHelper fileHelper)
        {
            _context = context;
            _fileHelper = fileHelper;
        }



        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Recipes.Include(r => r.ApprovalStatus).Include(r => r.Category).Include(r => r.Chef);
            return View(await modelContext.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.ApprovalStatus)
                .Include(r => r.Category)
                .Include(r => r.Chef)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "ApprovalStatusId");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe)
        {

            if (ModelState.IsValid)
            {
                if (recipe.ImageRecipeFile!=null) {
                    recipe.ImageRecipe = await _fileHelper.UploadFileAsync(recipe.ImageRecipeFile);
                    _context.Add(recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
              
            }

            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    // Log or print error messages to debug output
                    Debug.WriteLine(error.ErrorMessage);
                }
            
        
            }

            // Prepare ViewData for dropdown lists in the view
            ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "ApprovalStatusId", recipe.ApprovalStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recipe.CategoryId);
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipe.ChefId);

            // Return the view with the model to display validation errors
            return View(recipe);
        }


























        // 
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "ApprovalStatusId", recipe.ApprovalStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipe.ChefId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (recipe!=null)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeId))
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
            ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "ApprovalStatusId", recipe.ApprovalStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipe.ChefId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.ApprovalStatus)
                .Include(r => r.Category)
                .Include(r => r.Chef)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }
    }
}
