using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using task_one_v2.App_Core.Mail;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.Models;
using task_one_v2.ViewModel;


using task_one_v2.App_Core.FileHelper;
using System.Diagnostics;


namespace task_one_v2.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ModelContext _context;
        private readonly FileHelper _fileHelper;
        private readonly IAuthManager _authManager;
        private readonly IUserManager _userManger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer, ModelContext context, FileHelper fileHelper, IAuthManager authManager, IUserManager userManger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _localizer = localizer;
            _context = context;
            _fileHelper = fileHelper;
            _authManager = authManager;
            _userManger = userManger;
            _hostingEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Home()
        {
            ViewBag.Recipes = await _userManger.GetAllRecipe();
            ViewBag.chef = await _userManger.GetAllChef();
            ViewBag.Testimonial = await _userManger.GetAllTestimonial();

            var numberOfChef = await _userManger.GetAllChef();
            ViewBag.NumberOfChef = numberOfChef.Count();
            ViewBag.YearsOfExperience = _userManger != null ? _userManger.getYearsOfExperince() : 20;
            ViewBag.HomeImg = _context.Homepages.FirstOrDefault();
            return View();
        }

        public async Task<IActionResult> RecipeDetails(decimal? id)
        {
            if (id == null) return NotFound();
            var recipe = await _userManger.GetRecipeDependsOnId(id ?? 0);
            if (recipe == null) return NotFound();
            ViewBag.RecipeDetails = recipe;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecipeDetails(decimal? id, PurchaseViewModel purchaseViewModel)
        {
            if (id == null) return NotFound();
            var recipe = await _userManger.GetRecipeDependsOnId(id ?? 0);
            if (recipe == null) return NotFound();
            ViewBag.RecipeDetails = recipe;

            decimal? userID = _authManager.GetCurrentUserId();
            if (userID == null) return RedirectToAction("Login", "Auth");

            try
            {
                RecipeOrder recipeOrder = new RecipeOrder
                {
                    UserId = userID ?? 0,
                    RecipeId = recipe.RecipeId,
                    OrderDate = DateTime.Now,
                };

                Payment payment = await _context.Payments
                    .AsNoTracking()
                    .SingleOrDefaultAsync(check => check.Userid == userID);

                payment.Amount -= recipe.Price;



                await _context.AddAsync(recipeOrder);
                await _context.SaveChangesAsync();

                // Update payment
                _context.Update(payment);
                await _context.SaveChangesAsync();



                PdfData pdfData = new PdfData
                {
                    ChefName = recipe?.Chef?.FirstName + " " + recipe?.Chef?.LastName,
                    RecipeName = recipe?.Recipename ?? "",
                    Description = recipe?.Description ?? "",
                    CategoryName = recipe?.Category?.CategoryName ?? "",
                    Ingredients = recipe?.Ingredients ?? "",
                    Procedure = recipe?.Procedure ?? "",
                    RecipeImg = recipe?.ImageRecipe ?? ""
                };
                PDFHandler.GeneratePdfOrder(_hostingEnvironment, pdfData);
                string attachmentPath = Path.Combine(_hostingEnvironment.WebRootPath, "PDF", "Recipe.pdf");

                SendEmail.Send("abdullah.khraissat@gmail.com", "Recipe Details", "Please find attached recipe details.", attachmentPath);

                return RedirectToAction("MyOrder", "Home");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return RedirectToAction("Home", "Home");
            }
        }

        public async Task<IActionResult> ChefProfile(decimal? id)
        {
            if (id == null) return NotFound();
            var chef = await _userManger.GetChefDependsOnId(id ?? 0);
            if (chef == null) return NotFound();
            return View(chef);
        }




        public async Task<IActionResult> About()
        {
            var numberOfChef = await _userManger.GetAllChef();
            ViewBag.NumberOfChef = numberOfChef.Count();
            ViewBag.YearsOfExperience = _userManger != null ? _userManger.getYearsOfExperince() : 20;
            return View();
        }

        public async Task<IActionResult> Menu()
        {
            ViewBag.categories = await _userManger.GetAllCategory();
            ViewBag.firstCategoryID = _context?.Categories?.FirstOrDefault()?.CategoryId??0;


            ViewBag.Search = _userManger.searchRecipe();
            if (ViewBag.firstCategoryID==null) {
                RedirectToAction("Home","Home");
            }
            return View();
        }



        public async Task<IActionResult> OurTeam()
        {
            ViewBag.chef = await _userManger.GetAllChef();

            return View();
        }
        public async Task<IActionResult> Testimonial()
        {
            ViewBag.Testimonial = await _userManger.GetAllTestimonial();

            return View();
        }


        public IActionResult PrivicyPolicy()
        {
            return View();
        }



        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            return LocalRedirect(returnUrl);
        }


        ////////////////////////////////////////////////////////////////////////////
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
                return RedirectToAction(nameof(Home), "Home");
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
                return RedirectToAction(nameof(Home), "Home");
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> MyOrder()
        {
            decimal? userId = _authManager.GetCurrentUserId();
            if (userId == null) { RedirectToAction("Login", "Auth"); }
            List<RecipeOrder> orders = await _userManger?.GetAllUserOrder(userId);

            return View(orders);
        }
    }
}
