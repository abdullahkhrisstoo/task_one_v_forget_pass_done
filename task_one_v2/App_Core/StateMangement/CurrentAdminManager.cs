using task_one_v2.App_Core.ConstString;
using task_one_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using task_one_v2.App_Core.StateMangement;
using System.Collections.Generic;
using task_one_v2.ViewModel;
using iTextSharp.text;
using SkiaSharp;
using System.Data;

using System.Globalization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using task_one_v2.App_Core.FileHelper;
using System.Diagnostics;
namespace task_one_v2.App_Core.StateMangement
{


    public interface IAdminManager
    {
        Task<bool> CreateCategory(Category category, ModelStateDictionary check);
        Task<bool> EditCategory(decimal? id,Category category);
        Task<List<StatisticsInfo>> SalesStatisticsInfos();
        Task<List<StatisticsInfo>> RecipeStatisticsInfos();
        Task<List<UserInfo>> GetAllRegisterdAccount();
        Task<List<UserInfo>> GetAllUserDependsOnUserRole();
        Task<List<UserInfo>> GetAllChefDependsOnUserRole();
        Task<List<UserInfo>> GetAllPendingChef();
        Task<List<UserInfo>> GetAllRejectedChef();
        Task<List<UserInfo>> GetAlApprovedChef();
        Task<List<Recipe>> getAllRecipe();
        Task<List<Recipe>> GetAllPendingRecipe();
        Task<List<Recipe>> GetAllRejectedRecipe();
        Task<List<Recipe>> GetAlApprovedRecipe();
        Task<List<RecipeOrder>> getAllOrder();
        Task<List<Testimonial>> getAllTestimonials();
        Task<List<Testimonial>> GetAllPendingTestimonial();
        Task<List<Testimonial>> GetAllRejectedTestimonial();
        Task<List<Testimonial>> GetAlApprovedTestimonial();
        //count
        Task<int> GetCountUserRegisterdAccount();
        Task<int> GetCountUser();
        Task<int> GetCountChef();
        Task<int> GetCountChefPending();
        Task<int> GetCountChefApproved();
        Task<int> GetCountChefRejected();
        Task<int> GetCountOrder();
        Task<int> GetCountTestimonials();
        Task<int> GetCountPendingTestimonial();
        Task<int> GetCountApprovedTestimonial();
        Task<int> GetCountRejectedTestimonial();
        Task<List<Category>> GetAllCategory();
        Task<List<UserVerificationStatus>> GetetChefStatus();
        Task<UserInfo> GetChefDependsOnUserId(int id);


        Task<List<StatisticsInfo>> FullstatisticsInfos();
        Task<List<StatisticsInfo>> UserStatisticsInfos();

        Task<List<StatisticsInfo>> ChefStatisticsInfos();

        Task<List<StatisticsInfo>> TestimonialStatisticsInfos();

        ////////////////////////////////////////////////////////////////////////
        Task<int> getCountAllRecipe();
        Task<int> getCountAllPendingRecipe();
        Task<int> getCountAllRejectedRecipe();
        Task<int> getCountAllApprovedRecipe();

        Task<Recipe?> BestSellerRecipe();
        Task<Recipe?> LeastSellingRecipe();


        Task<double?> SalesLastDay();
        Task<double?> SalesLast48Hours();
        Task<double?> SalesLastWeek();

        Task<double?> SalesLastMonth();
        Task<double?> SalesLastYear();

        Task<double?> AllSales();
        Task<List<StatisticsInfo>> ProfitStatisticsInfos();




    }

    public class CurrentAdminManager : IAdminManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ModelContext _context;
        private readonly IAuthManager _authManager;
        private readonly ILogger<CurrentAdminManager> _logger;
        private readonly FileHelper.FileHelper _fileHelper;

        public CurrentAdminManager(IHttpContextAccessor httpContextAccessor, ModelContext context, IAuthManager authManager, ILogger<CurrentAdminManager> logger, FileHelper.FileHelper fileHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _authManager = authManager;
            _logger = logger;
            _fileHelper = fileHelper;
        }






        // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// get all user(admin, chef, user)
        /// </summary>
        /// <returns>
        /// list of (admin, chef, user)
        /// </returns>
        public async Task<List<UserInfo>> GetAllRegisterdAccount() =>
            await _context.UserInfos
                 .Include(u => u.UserCredentials)
                 .Include(recipe => recipe.Recipes)
                 .Include(o => o.RecipeOrders)
                 .Include(role => role.Role)
                 .Include(testimonial => testimonial.Testimonials)
                 .Include(status => status.VerificationStatus)
                 .ToListAsync();
        /// <summary>
        /// User(userdata) depends on user role id
        /// </summary>
        /// <returns>
        /// list of user 
        /// </returns>
        public async Task<List<UserInfo>> GetAllUserDependsOnUserRole()
        {
            var allUsers = await GetAllRegisterdAccount();
            var usersWithMatchingRole = allUsers.Where(user => user?.Role?.RoleId == ConstantApp.userRole).ToList();
            return usersWithMatchingRole;
        }

        /// <summary>
        /// User(chef faya) depends on user role id
        /// </summary>
        /// <returns>
        /// list of chef 
        /// </returns>
        public async Task<List<UserInfo>> GetAllChefDependsOnUserRole()
        {
            var allUsers = await GetAllRegisterdAccount();
            var usersWithMatchingRole = allUsers.Where(user => user?.Role?.RoleId == ConstantApp.chefRole).ToList();
            return usersWithMatchingRole;
        }

        /// <summary>
        /// PRIVATE methods depends on status Chef to get chef data
        /// </summary>
        /// <param name="verificationStatusId"></param>
        /// <returns>
        /// list of chef data
        /// </returns>
        private async Task<List<UserInfo>> GetAllChefsByVerificationStatus(int verificationStatusId)
        {
            var allChefs = await GetAllChefDependsOnUserRole();
            var chefsWithMatchingStatus = allChefs.Where(user => user?.VerificationStatus?.VerificationStatusId == verificationStatusId).ToList();
            return chefsWithMatchingStatus;
        }

        public async Task<List<UserInfo>> GetAllPendingChef() => await GetAllChefsByVerificationStatus(ConstantApp.pendingChef);


        public async Task<List<UserInfo>> GetAllRejectedChef() => await GetAllChefsByVerificationStatus(ConstantApp.rejectedChef);


        public async Task<List<UserInfo>> GetAlApprovedChef() => await GetAllChefsByVerificationStatus(ConstantApp.approvedChef);


        // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Recipe


        /// <summary>
        /// Get All Recipe
        /// </summary>
        /// <returns></returns>
        public async Task<List<Recipe>> getAllRecipe() =>
            await _context.Recipes
                 .Include(category => category.Category)
                 .Include(u => u.Chef)
                 .Include(status => status.ApprovalStatus)
                 .ToListAsync();



        /// <summary>
        /// PRIVATE methods depends on status Recipe to get recipe data
        /// </summary>
        /// <param name="verificationStatusId"></param>
        /// <returns>
        /// list of Recipe data
        /// </returns>
        private async Task<List<Recipe>> GetAllRecipeByVerificationStatus(int verificationStatusId)
        {
            var allRecipe = await getAllRecipe();
            var recipeWithMatchingStatus = allRecipe.Where(status => status.ApprovalStatus.ApprovalStatusId == verificationStatusId).ToList();
            return recipeWithMatchingStatus;
        }


        public async Task<List<Recipe>> GetAllPendingRecipe() => await GetAllRecipeByVerificationStatus(ConstantApp.pendingRecipe);


        public async Task<List<Recipe>> GetAllRejectedRecipe() => await GetAllRecipeByVerificationStatus(ConstantApp.rejectedRecipe);


        public async Task<List<Recipe>> GetAlApprovedRecipe() => await GetAllRecipeByVerificationStatus(ConstantApp.approvedRecipe);
        ///- ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //todo: Get All RecipeOrder



        /// <summary>
        /// Get All Order
        /// </summary>
        /// <returns></returns>
        public async Task<List<RecipeOrder>> getAllOrder() =>
            await _context.RecipeOrders
                 .Include(recipe => recipe.Recipe)
                 .Include(u => u.User)//include user 
                 .Include(c => c.Recipe.Chef)//include chef
                 .ToListAsync();





        ///- ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //todo: Get All Testmonial


        /// <summary>
        /// Get All Testimonial
        /// </summary>
        /// <returns></returns>
        public async Task<List<Testimonial>> getAllTestimonials() =>
            await _context.Testimonials
                 .Include(u => u.User)
                 .Include(s => s.ApprovalStatus)
                 .ToListAsync();




        private async Task<List<Testimonial>> GetAllTestimonialByVerificationStatus(int verificationStatusId)
        {
            var allTestimonial = await getAllTestimonials();
            var TestimonialWithMatchingStatus = allTestimonial.Where(status => status.ApprovalStatus.ApprovalStatusId == verificationStatusId).ToList();
            return TestimonialWithMatchingStatus;
        }


        public async Task<List<Testimonial>> GetAllPendingTestimonial() => await GetAllTestimonialByVerificationStatus(ConstantApp.pendingTestimonial);


        public async Task<List<Testimonial>> GetAllRejectedTestimonial() => await GetAllTestimonialByVerificationStatus(ConstantApp.rejectedTestimonial);


        public async Task<List<Testimonial>> GetAlApprovedTestimonial() => await GetAllTestimonialByVerificationStatus(ConstantApp.approvedTestimonial);






        public async Task<int> GetCountUserRegisterdAccount()
        {
            var data = await GetAllRegisterdAccount();
            return data.Count();
        }
        public async Task<int> GetCountUser()
        {
            var data = await GetAllUserDependsOnUserRole();
            return data.Count();
        }
        public async Task<int> GetCountChef()
        {
            var data = await GetAllChefDependsOnUserRole();
            return data.Count();
        }
        public async Task<int> GetCountChefPending()
        {
            var data = await GetAllPendingChef();
            return data.Count();
        }
        public async Task<int> GetCountChefApproved()
        {
            var data = await GetAlApprovedChef();
            return data.Count();
        }
        public async Task<int> GetCountChefRejected()
        {
            var data = await GetAllRejectedChef();
            return data.Count();
        }
        public async Task<int> GetCountOrder()
        {
            var data = await getAllOrder();
            return data.Count();
        }

        public async Task<int> GetCountTestimonials()
        {
            var data = await getAllTestimonials();
            return data.Count();
        }

        public async Task<int> GetCountPendingTestimonial()
        {
            var data = await GetAllPendingTestimonial();
            return data.Count();
        }

        public async Task<int> GetCountApprovedTestimonial()
        {
            var data = await GetAlApprovedTestimonial();
            return data.Count();
        }

        public async Task<int> GetCountRejectedTestimonial()
        {
            var data = await GetAllRejectedTestimonial();
            return data.Count();
        }


        //todo: get all categore
        public async Task<List<Category>> GetAllCategory() => await _context.Categories.ToListAsync();

        //chef status
        public async Task<List<UserVerificationStatus>> GetetChefStatus()
        {
            var status = await _context.UserVerificationStatuses.ToListAsync();
            return status;
        }

        public async Task<UserInfo> GetChefDependsOnUserId(int id)
        {
            var allChef = await GetAllChefDependsOnUserRole();
            return allChef!.SingleOrDefault(u => u?.Userid == id);
        }

        public async Task<List<StatisticsInfo>> FullstatisticsInfos()
        {
            List<StatisticsInfo> statisticsInfos = new List<StatisticsInfo>{
                new StatisticsInfo { GetCount = await GetCountUserRegisterdAccount(), Title = "All registered accounts" },
                new StatisticsInfo { GetCount = await GetCountUser()  , Title = "All users" },
                new StatisticsInfo { GetCount = await GetCountChef()  , Title = "All chefs" },
                new StatisticsInfo { GetCount = await GetCountChefPending()  , Title = "All pending chefs" },
                new StatisticsInfo { GetCount = await GetCountChefApproved()  , Title = "All approved chefs" },
                new StatisticsInfo { GetCount = await GetCountChefRejected()  , Title = "All rejected chefs" },
                new StatisticsInfo { GetCount = await GetCountOrder()  , Title = "All Order" },
                new StatisticsInfo { GetCount = await GetCountTestimonials()  , Title = "all testimonials" },
                new StatisticsInfo { GetCount = await GetCountPendingTestimonial()  , Title = "all pending testimonial" },
                new StatisticsInfo { GetCount = await GetCountApprovedTestimonial()  , Title =  "all approved testimonial" },
                new StatisticsInfo { GetCount = await GetCountRejectedTestimonial()  , Title = "all rejected testimonial" }
            };
            return statisticsInfos;
        }

        public async Task<List<StatisticsInfo>> UserStatisticsInfos()
        {
            List<StatisticsInfo> statisticsInfos = new List<StatisticsInfo>{
                new StatisticsInfo { GetCount = await GetCountUserRegisterdAccount(), Title = "All registered accounts" },
                new StatisticsInfo { GetCount = await GetCountUser(), Title = "All users" },
                new StatisticsInfo { GetCount = await GetCountChef(), Title = "All chefs" }, };
            return statisticsInfos;
        }

        public async Task<List<StatisticsInfo>> ChefStatisticsInfos()
        {
            List<StatisticsInfo> statisticsInfos = new List<StatisticsInfo>{
            new StatisticsInfo { GetCount = await GetCountChef(), Title = "All chefs" },
                new StatisticsInfo { GetCount = await GetCountChefPending(), Title = "All pending chefs" },
                new StatisticsInfo { GetCount = await GetCountChefApproved(), Title = "All approved chefs" },
                new StatisticsInfo { GetCount = await GetCountChefRejected(), Title = "All rejected chefs" },
                };
            return statisticsInfos;
        }

        public async Task<List<StatisticsInfo>> TestimonialStatisticsInfos()
        {
            List<StatisticsInfo> testimonialStatisticsInfos = new List<StatisticsInfo> {
                new StatisticsInfo { GetCount = await GetCountTestimonials()  , Title = "all testimonials" },
                new StatisticsInfo { GetCount = await GetCountPendingTestimonial()  , Title = "all pending testimonial" },
                new StatisticsInfo { GetCount = await GetCountApprovedTestimonial()  , Title =  "all approved testimonial" },
                new StatisticsInfo { GetCount = await GetCountRejectedTestimonial()  , Title = "all rejected testimonial" }
            };
            return testimonialStatisticsInfos;
        }


        //////////////////////////////////////////////////////////////////////////////////// /
        public async Task<int> getCountAllRecipe()
        {
            var data = await getAllRecipe();
            return data.Count();
        }
        public async Task<int> getCountAllPendingRecipe()
        {
            var data = await getAllRecipe();
            return data.Where(s => s?.ApprovalStatus?.ApprovalStatusId == ConstantApp.pendingRecipe).Count();
        }
        public async Task<int> getCountAllRejectedRecipe()
        {
            var data = await getAllRecipe();
            return data.Where(s => s?.ApprovalStatus?.ApprovalStatusId == ConstantApp.rejectedRecipe).Count();
        }

        public async Task<int> getCountAllApprovedRecipe()
        {
            var data = await getAllRecipe();
            return data.Where(s => s?.ApprovalStatus?.ApprovalStatusId == ConstantApp.approvedRecipe).Count();
        }

        public async Task<Recipe?> BestSellerRecipe()
        {
            var recipes = await getAllRecipe();
            var bestSeller = recipes.OrderByDescending(r => r.RecipeOrders).FirstOrDefault();
            return bestSeller;
        }


        public async Task<Recipe?> LeastSellingRecipe()
        {
            var recipes = await getAllRecipe();
            var leastSelling = recipes.OrderBy(r => r.RecipeOrders).FirstOrDefault();
            return leastSelling;
        }

        public async Task<double?> SalesLastWeek()
        {
            DateTime lastWeekStart = DateTime.Today.AddDays(-7);
            DateTime lastWeekEnd = DateTime.Today;

            return await CalculateSalesInRange(lastWeekStart, lastWeekEnd);
        }

        public async Task<double?> SalesLastDay()
        {
            DateTime lastDayStart = DateTime.Today.AddDays(-1);
            DateTime lastDayEnd = DateTime.Today;

            return await CalculateSalesInRange(lastDayStart, lastDayEnd);
        }

        public async Task<double?> SalesLast48Hours()
        {
            DateTime last48HoursStart = DateTime.Now.AddHours(-48);
            DateTime last48HoursEnd = DateTime.Now;

            return await CalculateSalesInRange(last48HoursStart, last48HoursEnd);
        }

        public async Task<double?> SalesLastMonth()
        {
            DateTime lastMonthStart = DateTime.Today.AddMonths(-1);
            DateTime lastMonthEnd = DateTime.Today;

            return await CalculateSalesInRange(lastMonthStart, lastMonthEnd);
        }

        public async Task<double?> SalesLastYear()
        {
            DateTime lastYearStart = DateTime.Today.AddYears(-1);
            DateTime lastYearEnd = DateTime.Today;

            return await CalculateSalesInRange(lastYearStart, lastYearEnd);
        }

        private async Task<double> LogAndReturnSalesData(Func<Task<double?>> salesFunction, string label)
        {
            double? sales = await salesFunction();
            if (sales.HasValue)
            {
                _logger.LogInformation($"{label}: {sales.Value}");
                return sales.Value;
            }
            else
            {
                _logger.LogWarning($"{label}: No sales data found.");
                return 0;
            }
        }

        private async Task<double?> CalculateSalesInRange(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Calculating sales from {startDate} to {endDate}");

            var orders = await _context.RecipeOrders
                                       .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                                       .ToListAsync();

            if (orders == null || !orders.Any())
            {
                _logger.LogWarning("No orders found in the specified date range.");
                return null;
            }

            decimal totalSales = orders.Sum(o => o.TotalPrice ?? 0);
            _logger.LogInformation($"Total sales calculated: {totalSales}");

            return (double)totalSales;
        }

        public async Task<double?> AllSales()
        {
            var orders = await _context.RecipeOrders.ToListAsync();

            if (orders == null || !orders.Any())
                return null;

            decimal totalSales = orders.Sum(o => o.TotalPrice ?? 0);

            return (double)totalSales;
        }


        public async Task<List<StatisticsInfo>> SalesStatisticsInfos()
        {
            List<StatisticsInfo> RecipeStatisticsInfos = new List<StatisticsInfo> {
                new StatisticsInfo { GetCount = await AllSales()??0  , Title = "All Sales" },
                new StatisticsInfo { GetCount = await SalesLastYear()??0  , Title = "Sales Last Year" },
                new StatisticsInfo { GetCount = await SalesLastMonth()??0  , Title =  "Sales Last Month" },
                new StatisticsInfo { GetCount = await SalesLast48Hours()??0  , Title = "Sales Last 48 Hours" },
                new StatisticsInfo { GetCount = await SalesLastDay() ??0 , Title = "Sales Last Day" },
                new StatisticsInfo { GetCount = await SalesLastWeek() ??0 , Title = "Sales Last Week" },

            };
            return RecipeStatisticsInfos;
        }

        public async Task<List<StatisticsInfo>> RecipeStatisticsInfos()
        {
            List<StatisticsInfo> RecipeStatisticsInfos = new List<StatisticsInfo>
            {
                new StatisticsInfo { GetCount = await getCountAllRecipe()  , Title = "all Recipe" },
                new StatisticsInfo { GetCount = await getCountAllPendingRecipe()  , Title = "all pending Recipe" },
                new StatisticsInfo { GetCount =  await getCountAllApprovedRecipe(), Title =  "all approved Recipe" },
                new StatisticsInfo { GetCount =  await getCountAllRejectedRecipe()  , Title = "all rejected Recipe" }
            };
            return RecipeStatisticsInfos;
        }




        public async Task<double?> CalculateProfitInRange(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Calculating profits from {startDate} to {endDate}");

            var orders = await _context.RecipeOrders
                                       .Include(ro => ro.Recipe)
                                       .Where(ro => ro.OrderDate >= startDate && ro.OrderDate <= endDate)
                                       .ToListAsync();

            if (orders == null || !orders.Any())
            {
                _logger.LogWarning("No orders found in the specified date range.");
                return null;
            }

            decimal totalProfit = orders.Sum(o => o.Recipe?.Price ?? 0);
            _logger.LogInformation($"Total profit calculated: {totalProfit}");

            return (double)totalProfit;
        }

        public async Task<double?> ProfitLastWeek()
        {
            DateTime lastWeekStart = DateTime.Today.AddDays(-7);
            DateTime lastWeekEnd = DateTime.Today;
            return await CalculateProfitInRange(lastWeekStart, lastWeekEnd);
        }

        public async Task<double?> ProfitLastDay()
        {
            DateTime lastDayStart = DateTime.Today.AddDays(-1);
            DateTime lastDayEnd = DateTime.Today;
            return await CalculateProfitInRange(lastDayStart, lastDayEnd);
        }

        public async Task<double?> ProfitLast48Hours()
        {
            DateTime last48HoursStart = DateTime.Now.AddHours(-48);
            DateTime last48HoursEnd = DateTime.Now;
            return await CalculateProfitInRange(last48HoursStart, last48HoursEnd);
        }

        public async Task<double?> ProfitLastMonth()
        {
            DateTime lastMonthStart = DateTime.Today.AddMonths(-1);
            DateTime lastMonthEnd = DateTime.Today;
            return await CalculateProfitInRange(lastMonthStart, lastMonthEnd);
        }

        public async Task<double?> ProfitLastYear()
        {
            DateTime lastYearStart = DateTime.Today.AddYears(-1);
            DateTime lastYearEnd = DateTime.Today;
            return await CalculateProfitInRange(lastYearStart, lastYearEnd);
        }

        public async Task<double?> AllProfits()
        {
            var orders = await _context.RecipeOrders
                                       .Include(ro => ro.Recipe)
                                       .ToListAsync();

            if (orders == null || !orders.Any())
            {
                _logger.LogWarning("No orders found.");
                return null;
            }

            decimal totalProfit = orders.Sum(o => o.Recipe?.Price ?? 0);
            _logger.LogInformation($"Total profit calculated: {totalProfit}");

            return (double)totalProfit;
        }

        public async Task<List<StatisticsInfo>> ProfitStatisticsInfos()
        {
         List<StatisticsInfo> profitStatisticsInfos = new List<StatisticsInfo> {
        new StatisticsInfo { GetCount = await AllProfits() ?? 0, Title = "All Profits" },
        new StatisticsInfo { GetCount = await ProfitLastYear() ?? 0, Title = "Profits Last Years" },
        new StatisticsInfo { GetCount = await ProfitLastMonth() ?? 0, Title = "Profits Last Month" },
        new StatisticsInfo { GetCount = await ProfitLast48Hours() ?? 0, Title = "Profits Last 48 Hours" },
        new StatisticsInfo { GetCount = await ProfitLastDay() ?? 0, Title = "Profits Last Day" },
        new StatisticsInfo { GetCount = await ProfitLastWeek() ?? 0, Title =  "Profits Last Week" }
    };
            return profitStatisticsInfos;
        }

        public async Task<bool> CreateCategory(Category category, ModelStateDictionary check)
        {
            if (!check.IsValid) return false;
            if (category.FormFile == null) return false;

            category.ImageCategory = await _fileHelper.UploadFileAsync(category.FormFile);
            _context.Add(category);
            await _context.SaveChangesAsync();
            return true;



        }

        public async Task<bool> EditCategory(decimal? id, Category category)
        {
            if (id == null) return false;
            category.CategoryId = id ?? 0;


            if (category.FormFile == null) { return false; }

            category.ImageCategory = await _fileHelper.UploadFileAsync(category.FormFile);

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Debug.WriteLine(e);
                return false;
            }
            return true;

        }
    }
}





