using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizApp.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;

namespace QuizApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public INotyfService _notifyService { get; }
        private readonly testContext _context;

        public HomeController(ILogger<HomeController> logger, testContext context, INotyfService notifyService)
        {
            _logger = logger;
            _context = context;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            ViewBag.Gender = Utilities.GenderList();
            if (Request.Cookies["_id"] == null)
            {
                return View();
            }
            var check = _context.UserInfos
                .Where(x => x.UserId == int.Parse(Request.Cookies["_id"]))
                .FirstOrDefault();
            if (check == null) return View();
            return RedirectToAction("Index", "Dashboard", new { area = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostUsers([Bind("UserId,FullName,Gender,Birthday,IdNum,Address,PhoneNum,Email,DateCreated")] UserInfo user)
        {
            var existPhoneNum = _context.UserInfos.Where(x => x.PhoneNum == user.PhoneNum).AsNoTracking().FirstOrDefault();
            var existIdNum = _context.UserInfos.Where(x => x.IdNum == user.IdNum).AsNoTracking().FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (existIdNum != null || existPhoneNum != null)
                {
                    _notifyService.Error("Thông tin đã được sử dụng");
                }
                ViewBag.Gender = Utilities.GenderList();
                user.DateCreated = DateTime.Now.ToString("dd-MM-yyyy");
                _context.Add(user);
                await _context.SaveChangesAsync();
                CookieOptions options = new CookieOptions();
                options.Expires = new DateTimeOffset(2038, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));
                Response.Cookies.Append("_id", user.UserId.ToString(), options);
                Response.Cookies.Append("_phoneNum", user.PhoneNum, options);
                Response.Cookies.Append("_idNum", user.IdNum, options);
                Response.Cookies.Append("_gender", user.Gender, options);
                return RedirectToAction("Index", "Dashboard", new { area = "", id = user.UserId });
            }
            _notifyService.Error("Vui lòng nhập đầy đủ thông tin");
            return RedirectToAction("Index","Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
