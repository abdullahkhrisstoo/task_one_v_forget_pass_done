using task_one_v2.App_Core.ConstString;
using task_one_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using task_one_v2.App_Core.StateMangement;
using System.Collections.Generic;
namespace task_one_v2.App_Core.StateMangement
{
    public interface ICurrentChefManager
    {
        int? GetCurrentUserId();
        UserInfo? GetCurrentUserData();
        Task<List<Category>> GetAllCategory();
        Task<List<Recipe>> GetRecipesByCategoryId(int categoryId);

        Task<List<UserInfo>> GetAllChef();
        Task<List<Recipe>> GetRecipeByChefId(int id);
        Task<List<Recipe>> GetAllRecipe();
        Task<List<Recipe>> GetRecipeUseSearch(string search);

 
    }

    public class CurrentChefManager : ICurrentChefManager
    {
        int approvedRecipe = 3;
        int approvedChef = 3;
        int chefRole = 3;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ModelContext _context;

        public CurrentChefManager(IHttpContextAccessor httpContextAccessor, ModelContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public int? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.Session.GetInt32(ConstantApp.userSessionKey);
        }

        public UserInfo? GetCurrentUserData()
        {
            var userId = GetCurrentUserId();
            if (userId != null)
            {
                return _context.UserInfos.SingleOrDefault(u => u.Userid == userId);
            }
            return null;
        }


        //todo: get all categore
        public async Task<List<Category>> GetAllCategory()=> await _context.Categories.ToListAsync();


        //get recipe depends cat id
        public async Task<List<Recipe>> GetRecipesByCategoryId(int categoryId)
        => await _context.Recipes.Include(s=>s.ApprovalStatus)
            .Include(c=> c.Category)
            .Include(c=>c.Chef)
            .Where(recipe => recipe.CategoryId == categoryId)
            .Where(state => state.ApprovalStatus.ApprovalStatusId == approvedRecipe)
            .ToListAsync();

        //todo: get all Cehf
        public async Task<List<UserInfo>> GetAllChef()
            => await _context.UserInfos
            .Include(r=>r.Role)
            .Include(s=>s.VerificationStatus)
            .Include(r=>r.Recipes)
            .Where(r=>r.Role.RoleId== chefRole)
            .Where(state=>state.VerificationStatus.VerificationStatusId==approvedChef)
            .ToListAsync();


        //todo: get all recipe By Chef Id
        public async Task<List<Recipe>> GetRecipeByChefId(int id)
            => await _context.Recipes
            .Include(r => r.ApprovalStatus)
            .Include(c => c.Category)
            .Include(chef => chef.Chef)
            .Where(verify=>verify.ApprovalStatus.ApprovalStatusId== approvedRecipe)
            .Where(recipe=>recipe.Chef.Userid==id)
            .ToListAsync();

        public async Task<List<Recipe>> GetRecipeUseSearch(string search)
          => await _context.Recipes
             .Include(r => r.ApprovalStatus)
             .Include(c => c.Category)
             .Include(chef => chef.Chef)
             .Where(verify => verify.ApprovalStatus.ApprovalStatusId == approvedRecipe)
             .Where(r => r.Recipename.ToLower().Contains(search.ToLower()))
             .ToListAsync();

        public async Task<List<Recipe>> GetAllRecipe()
        => await _context.Recipes
             .Include(r => r.ApprovalStatus)
             .Include(c => c.Category)
             .Include(chef => chef.Chef)
             .Where(verify => verify!.ApprovalStatus!.ApprovalStatusId == approvedRecipe)
             .ToListAsync();
    }


}
