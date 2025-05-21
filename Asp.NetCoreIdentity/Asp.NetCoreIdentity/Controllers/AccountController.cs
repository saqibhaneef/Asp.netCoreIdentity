using Asp.NetCoreIdentity.Data;
using Asp.NetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result2 = await signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);

                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (result2.RequiresTwoFactor)
                    {

                    }

                    if (result2.IsLockedOut)
                    {

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid attempt.");
                        return View(model);
                    }
                }                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                

                var result = await this.userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errorMessage: item.Description);
                }
            }

            return View(model);
        }
    }
}
