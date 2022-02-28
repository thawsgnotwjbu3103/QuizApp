using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizApp.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace QuizApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly testContext _context;
        public DashboardController(testContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            int id = int.Parse(Request.Cookies["_id"]);
            var check = _context.UserInfos
                .Where(x => x.UserId == id)
                .FirstOrDefault();
            if (check == null) return RedirectToAction("Index", "Home", new { area = "" });


            ViewBag.Check = _context.DisableLists.Where(x=>x.UserId == id).ToList();
            ViewBag.Gender = check.Gender;
            ViewBag.PhoneNum = check.PhoneNum;
            ViewBag.IdNum = check.IdNum;
            ViewBag.Notifications = await _context.Notifications.Where(x => x.IsActive).ToListAsync();
            ViewBag.Quizs = await _context.TblQuizzes.Where(x => x.IsActive).ToListAsync();
            return View();
        }

        public IActionResult NotifyDetails(int id) {
            if (Request.Cookies["_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            var check = _context.UserInfos
                .Where(x => x.UserId == int.Parse(Request.Cookies["_id"]))
                .FirstOrDefault();
            if (check == null) return RedirectToAction("Index", "Home", new { area = "" });

            var content = _context.Notifications.Where(x => x.NotifyId == id).FirstOrDefault();
            return View(content);
        } 


        [HttpPost]
        public IActionResult Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
