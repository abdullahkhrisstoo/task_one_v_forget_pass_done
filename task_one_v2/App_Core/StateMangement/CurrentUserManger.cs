using task_one_v2.App_Core.ConstString;
using task_one_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using task_one_v2.App_Core.StateMangement;
using System.Collections.Generic;
using task_one_v2.ViewModel;
using System.Diagnostics;

namespace task_one_v2.App_Core.StateMangement
{


    public interface IUserManager
    {
        Task<List<Category>> GetAllCategory();
        Task<List<Recipe>> GetAllRecipeDependsOnCategoryId(decimal id);
        Task<List<UserInfo>> GetAllChef();
        Task<List<Recipe>> GetAllRecipeDependsOnChefId(decimal id);
        Task<List<Recipe>> GetAllRecipe();
        Task<List<Testimonial>> GetAllTestimonial();
        Task<Recipe?> GetRecipeDependsOnId(decimal id);
        Task<bool> SendContactUs(ContactU contactU);

        Task<UserInfo?> GetChefDependsOnId(decimal id);

        Task<List<RecipeOrder?>>? GetAllUserOrder(decimal? id);
        Task<List<Dictionary<decimal, string>?>>? searchRecipe();
        int getYearsOfExperince();


    }
    public class CurrentUserManger : IUserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ModelContext _context;

        public CurrentUserManger(IHttpContextAccessor httpContextAccessor, ModelContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<List<Category>> GetAllCategory() =>
              _context.Categories != null ?
              await _context.Categories.ToListAsync() :
              null;
        public async Task<List<Recipe>> GetAllRecipeDependsOnCategoryId(decimal id)
        {
            var recipe = await GetAllRecipe();
            recipe = recipe
                .Where(x => x.CategoryId == id).ToList();
            return recipe;
        }
        public async Task<List<UserInfo>> GetAllChef()
        {
            var res = await _context.UserInfos
                   .Include(r => r.Role)
                   .Include(r => r.Recipes)
                   .Include(s => s.VerificationStatus)
                   .Where(r => r.Role.RoleId == ConstantApp.chefRole)
                   .Where(s => s.VerificationStatus.VerificationStatusId == ConstantApp.approvedChef)
                   .Where(check => check.VerificationStatus.VerificationStatusId == ConstantApp.approvedRecipe)
                   .ToListAsync();
            return res;

        }
        public async Task<List<Recipe>> GetAllRecipeDependsOnChefId(decimal id)
        {
            var recipe = await GetAllRecipe();
            recipe = recipe
                .Where(x => x.ChefId == id).ToList();
            return recipe;
        }
        public async Task<List<Recipe>> GetAllRecipe()
        {
            var recipe = await _context.Recipes
                .Include(status => status.ApprovalStatus)
                .Include(chef => chef.Chef)
                .Include(o => o.RecipeOrders)
                .Include(c => c.Category)
                 .Where(status => status.ApprovalStatus.ApprovalStatusId == ConstantApp.approvedRecipe)
                .ToListAsync();
            return recipe;

        }
        public async Task<List<Testimonial>> GetAllTestimonial()
        {
            var res = await _context.Testimonials
                .Include(s => s.ApprovalStatus)
                .Include(u => u.User)
                .Where(check => check.ApprovalStatus.ApprovalStatusId == ConstantApp.approvedTestimonial)
                .ToListAsync();
            return res;
        }
        public async Task<Recipe?> GetRecipeDependsOnId(decimal id)
        {
            var recipes = await GetAllRecipe();
            return recipes.SingleOrDefault(check => check.RecipeId == id) ?? null;
        }

        public async Task<UserInfo?> GetChefDependsOnId(decimal id)
        {
            var allChefs = await GetAllChef();
            return allChefs.SingleOrDefault(check => check.Userid == id);
        }

        public async Task<bool> SendContactUs(ContactU contactU)
        {
            if (contactU != null)
            {
                try
                {
                    await _context.AddAsync(contactU);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return false;
                }
            }
            return false;
        }

        public async Task<List<RecipeOrder?>>? GetAllUserOrder(decimal? id)
        {
            if (id == null) return null;
            var res = await _context.RecipeOrders
                .Include(r => r.Recipe)
                .Include(u => u.User)
                .Include(u => u.Recipe.Category)
                .Include(c => c.Recipe.Chef)
                .Where(check => check.UserId == id)
                .ToListAsync();
            return res;
        }

        public int getYearsOfExperince()
        {
            if (ConstantApp.startDate == null)
            {
                return 20;
            }
            else
            {
                TimeSpan experience = DateTime.Now - ConstantApp.startDate;
                int yearsOfExperience = (int)(experience.TotalDays / 365);
                return yearsOfExperience;
            }
        }

        public async Task<List<Dictionary<decimal, string>>> searchRecipe()
        {
            var recipes = await GetAllRecipe();
            var result = recipes.Select(recipe => new Dictionary<decimal, string>
                {{ recipe.RecipeId, recipe.Recipename }}).ToList();
                return result;
        }

    }
}



