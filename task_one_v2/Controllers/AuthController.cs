using Microsoft.AspNetCore.Mvc;
using task_one_v2.Models;
using Microsoft.Extensions.Localization;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.ViewModel;
namespace task_one_v2.Controllers
{
    public class AuthController : Controller
    {
        private static Dictionary<string, string> _passwordResetTokens = new Dictionary<string, string>();
        private readonly ModelContext _context;
        private readonly IStringLocalizer<AuthController> _localizer;
        private readonly IAuthManager _authManager;
        private readonly IEmailService _emailService;

        public AuthController(ModelContext context, IStringLocalizer<AuthController> localizer, IAuthManager authManager, IEmailService emailService)
        {
            _context = context;
            _localizer = localizer;
            _authManager = authManager;
            _emailService = emailService;
        }

        public IActionResult Login()=>View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return View(userLogin);

            AuthResult loginResult = await _authManager.LoginAsync(userLogin);

            if (loginResult.Success) return RedirectToAction(loginResult.RedirectAction, loginResult.RedirectController);
            else ModelState.AddModelError("", loginResult.ErrorMessage);

            return View(userLogin);
        }
        public IActionResult Register()=>View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register( UserDataRegisterViewModel userData)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await _authManager.RegisterAsync(userData);

                if (registerResult.Success)
                    return RedirectToAction(registerResult.RedirectAction, registerResult.RedirectController);
                else
                    ModelState.AddModelError("", registerResult.ErrorMessage);
                
            }

            return View(userData);
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View(nameof(Login));
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authManager.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = Guid.NewGuid().ToString();
                    _passwordResetTokens[token] = model.Email;

                    var resetLink = Url.Action("ResetPassword", "Auth", new { token }, Request.Scheme);
                    await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || !_passwordResetTokens.ContainsKey(token))
            {
                return RedirectToAction("Error", "Home");
            }

            var model = new ResetPasswordViewModel { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_passwordResetTokens.TryGetValue(model.Token, out var email))
                {
                    var user = await _authManager.GetUserByEmailAsync(email);
                    if (user != null)
                    {
                        await _authManager.UpdatePasswordAsyncForget(user.Id, model.NewPassword);
                        _passwordResetTokens.Remove(model.Token);
                        return RedirectToAction("ResetPasswordConfirmation");
                    }
                }

                ModelState.AddModelError("", "Invalid token");
            }
            return View(model);
        }
        public IActionResult ResetPasswordConfirmation() => View();
        public IActionResult ForgotPasswordConfirmation() => View();



    }
}
