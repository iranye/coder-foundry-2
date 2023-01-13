using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "App");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logout Failed: {ex}");
                return BadRequest("Logout Failed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            Redirect(Request.Query["ReturnUrl"].First());
                        }
                        else
                        {
                            return RedirectToAction("Home", "Index");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login Failed: {ex}");
            }
            ModelState.AddModelError("", "Failed to login");
            return View();
        }
    }
}
