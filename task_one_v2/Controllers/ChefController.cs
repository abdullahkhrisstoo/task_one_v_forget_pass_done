using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using task_one_v2.App_Core.ConstString;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.Models;
using task_one_v2.ViewModel;
using task_one_v2.App_Core.FileHelper;

namespace task_one_v2.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;
        private readonly FileHelper _fileHelper;
        private readonly ICurrentChefManager _currentChefManager;
        private readonly IAuthManager _authManager;

        public ChefController(ModelContext context, FileHelper fileHelper, ICurrentChefManager currentChefManager, IAuthManager authManager)
        {
            _context = context;
            _fileHelper = fileHelper;
            _currentChefManager = currentChefManager;
            _authManager = authManager;
        }

        
        public async Task<IActionResult> MyRecipe()
        {
            int id = (int)_authManager.GetCurrentUserId();
            if (id==null) { return RedirectToAction("Login","Auth"); }
            ViewBag.currentRecipe = await _currentChefManager.GetRecipeByChefId(id);

            return View();
        }

      




        public async Task<IActionResult> AllCategory()
        {
            ViewBag.categories = await _currentChefManager.GetAllCategory();
            return View();

        }
        public async Task<IActionResult> GetRecipeByCategoryId(int id)
        {
            ViewBag.recpies = await _currentChefManager.GetRecipesByCategoryId(id);
            return View();
        }



        public async Task<IActionResult> AllChef()
        {
            ViewBag.AllCef = await _currentChefManager.GetAllChef();
            return View();

        }
        public async Task<IActionResult> GetRecipeByChefId(int id)
        {
            ViewBag.recpies = await _currentChefManager.GetRecipeByChefId(id);
            return View();
        }










        [HttpGet]
        public async Task<IActionResult> SearchRecipe()
        {
           return View(await _currentChefManager.GetAllRecipe());
        }

        [HttpPost]
        public async Task<IActionResult> SearchRecipe(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.search = await _currentChefManager.GetRecipeUseSearch(searchString);
            }
            else
            {
                ViewBag.search = null;
            }

            return View();
        }






        [HttpGet]
        public async Task<IActionResult> AddRecipe()
        {

           ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "ApprovalStatusId");
           ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
           ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid");
           return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddRecipe(Recipe recipe)
        {

            if (_authManager.GetCurrentUserId()!=null)
            {
                recipe.ChefId = _authManager.GetCurrentUserId()??0;
                recipe.ApprovalStatusId = ConstantApp.pendingRecipe;
            }
            else {
                RedirectToAction("Error");
            }

            if (ModelState.IsValid)
            {
                if (recipe.ImageRecipeFile != null)
                {
                    recipe.ImageRecipe = await _fileHelper.UploadFileAsync(recipe.ImageRecipeFile);
                    _context.Add(recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyRecipe));
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipe.ChefId);

            // Return the view with the model to display validation errors
            return View(recipe);
        }


        [HttpPost]
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
            return RedirectToAction(nameof(MyRecipe));
        }

        ////------------------------------------------------------------------
        //edit
        [HttpGet]
        public async Task<IActionResult> EditRecipe(decimal? id)
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recipe.CategoryId);
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipe.ChefId);
            return View(recipe);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(decimal id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(MyRecipe));
            }
            ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "ApprovalStatusId", recipe.ApprovalStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recipe.CategoryId);
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipe.ChefId);
            return View(recipe);
        }

        public IActionResult Profile()
        {
            var user = _authManager.GetCredentialDataAndUserInfo();
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UpdateUserDataViewModel updateAdminProfileViewModel)
        {
            var user = _authManager.GetCredentialDataAndUserInfo();
            if (user == null) return NotFound();
            var result = await _authManager.UpdateProfileAsync(updateAdminProfileViewModel);

            if (result.Success)
                return RedirectToAction(nameof(MyRecipe), "Chef");
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult EditPassword()
        {
            var user = _authManager.GetCredentialDataAndUserInfo();
            if (user == null) return NotFound();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel editPasswordViewModel)
        {
            var user = _authManager.GetCredentialDataAndUserInfo();
            if (user == null) return NotFound();

            var result = await _authManager.UpdatePasswordAsync(editPasswordViewModel);
            if (result.Success)
                return RedirectToAction(nameof(MyRecipe), "Chef");
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View();
            }

        }


    }


}
