using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System;
using task_one_v2.App_Core.ConstString;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.Models;
using task_one_v2.ViewModel;
using task_one_v2.App_Core.extension;
using System.Diagnostics;
using task_one_v2.App_Core.FileHelper;
namespace task_one_v2.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IAdminManager _adminManager;
        private readonly FileHelper _fileHelper;
        private readonly IAuthManager _authManager;

        public AdminController(ModelContext context, IAdminManager adminManager, FileHelper fileHelper, IAuthManager authManager)
        {
            _context = context;
            _adminManager = adminManager;
            _fileHelper = fileHelper;
            _authManager = authManager;
        }

        public IActionResult Language()
        {

            return RedirectToAction("Statistics", "Localization");
        }


        public async Task<IActionResult> Statistics()
        {
            var chefStatisticsInfos = await _adminManager.ChefStatisticsInfos();
            var userStatisticsInfos = await _adminManager.UserStatisticsInfos();
            var testimonialStatisticsInfos = await _adminManager.TestimonialStatisticsInfos();
            var recipeStatisticsInfos = await _adminManager.RecipeStatisticsInfos();
            var salesStatisticsInfos = await _adminManager.SalesStatisticsInfos();
            var profitStatisticsInfos = await _adminManager.ProfitStatisticsInfos();
            var fullstatisticsInfos = await _adminManager.FullstatisticsInfos();

            ViewBag.ChefStatisticsInfos = chefStatisticsInfos;
            ViewBag.UserStatisticsInfos = userStatisticsInfos;
            ViewBag.TestimonialStatisticsInfos = testimonialStatisticsInfos;
            ViewBag.RecipeStatisticsInfos = recipeStatisticsInfos;
            ViewBag.SalesStatisticsInfos = salesStatisticsInfos;
            ViewBag.ProfitStatisticsInfos = profitStatisticsInfos;
            ViewBag.fullstatisticsInfos = fullstatisticsInfos;


            var currentUserData = _authManager.GetCurrentUserData();
            ViewBag.AdminData = currentUserData;

            return View();
        }



        public async Task<IActionResult> AllChef()
        {
            var chef =await _adminManager.GetAllChefDependsOnUserRole();


               



            ViewBag.AllChef = chef;
            return View();
        }
        public async Task<IActionResult> EditChefStatus(decimal? id)
        {
            if (id == null || _context.UserInfos == null) return NotFound();
            var userInfo = await _context.UserInfos.FindAsync(id);
            if (userInfo == null) return NotFound();

            ViewData["Roleid"] = new SelectList(_context.Roles, "RoleId", "RoleName", userInfo.Roleid);
            ViewData["VerificationStatusId"] = new SelectList(_context.UserVerificationStatuses, "VerificationStatusId", "StatusName", userInfo.VerificationStatusId);
            return View(userInfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChefStatus(decimal id, UserInfo userInfo)
        {
            if (id != userInfo.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Statistics));
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "RoleId", "RoleId", userInfo.Roleid);
            ViewData["VerificationStatusId"] = new SelectList(_context.UserVerificationStatuses, "VerificationStatusId", "VerificationStatusId", userInfo.VerificationStatusId);
            return View(userInfo);
        }

        public async Task<IActionResult> AllCustomer()
        {

            ViewBag.AllCustomer = await _adminManager.GetAllUserDependsOnUserRole();

            return View();
        }
        public async Task<IActionResult> AllRecipe(int status = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            var allRecipes = await _adminManager.getAllRecipe();

            switch (status)
            {
                case ConstantApp.pendingRecipe:
                    allRecipes = await _adminManager.GetAllPendingRecipe();
                    break;
                case ConstantApp.rejectedRecipe:
                    allRecipes = await _adminManager.GetAllRejectedRecipe();
                    break;
                case ConstantApp.approvedRecipe:
                    allRecipes = await _adminManager.GetAlApprovedRecipe();
                    break;
            }

            if (startDate != null && endDate != null)
                allRecipes = allRecipes.Where(d => d.CreationDate.Date >= startDate && d.CreationDate.Date <= endDate).ToList();

            if (startDate != null)
                allRecipes = allRecipes.Where(d => d.CreationDate.Date >= startDate).ToList();

            if (endDate != null)
                allRecipes = allRecipes.Where(d => d.CreationDate.Date <= endDate).ToList();


            ViewBag.AllRecipe = allRecipes.ToList();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RecipeDetails(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            List<Recipe> allRecipe = await _adminManager.getAllRecipe();
            var recipe = allRecipe.SingleOrDefault(r => r.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "StatusName", recipe.ApprovalStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["ChefId"] = new SelectList(_context.UserInfos, "Userid", "Userid", recipe.ChefId);
            return View(recipe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecipeDetails(decimal id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }


            if (recipe != null && recipe.ChefId != null && recipe.ApprovalStatusId != null && recipe.CategoryId != null)
            {

                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                    _context.Entry(recipe).State = EntityState.Detached;
                }

                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(AllRecipe));
            }
            ViewData["ApprovalStatusId"] = new SelectList(_context.RecipeApprovalStatuses, "ApprovalStatusId", "StatusName", recipe.ApprovalStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
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
                return RedirectToAction(nameof(Statistics), "Admin");
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
                return RedirectToAction(nameof(Statistics), "Admin");
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View();
            }

        }


        
    }

}

