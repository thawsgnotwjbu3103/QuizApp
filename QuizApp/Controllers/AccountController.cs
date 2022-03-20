using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public INotyfService _notifyService { get; }
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(INotyfService notifyService,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _notifyService = notifyService;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        //[AllowAnonymous]
        //public IActionResult Register() => View();

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register ([Bind("username, email, password")] Admin admin)
        //{
        //    if (ModelState.IsValid) 
        //    {
        //        IdentityUser user = new IdentityUser
        //        {
        //            UserName = admin.username,
        //        };
        //        IdentityResult result = await _userManager.CreateAsync(user, admin.password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Login");
        //        }
        //        foreach(IdentityError err in result.Errors)
        //        {
        //            ModelState.AddModelError("", err.Description);
        //        }
        //    }
        //    return View(admin);
        //}

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            Login login = new Login
            {
                ReturnUrl = ReturnUrl
            };
            return View(login);
        }


        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Password, ReturnUrl")] Login login)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(login.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
                _notifyService.Error("Có lỗi xảy ra, vui lòng thử lại sau");
            }
            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
