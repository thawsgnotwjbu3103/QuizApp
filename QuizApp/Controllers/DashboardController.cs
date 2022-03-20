using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System.Linq;
using System.Threading.Tasks;

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
            ViewBag.Gender = check.Gender;
            ViewBag.PhoneNum = check.PhoneNum;
            ViewBag.IdNum = check.IdNum;
            ViewBag.Notifications = await _context.Notifications.Where(x => x.IsActive).ToListAsync();
            ViewBag.Quizs = from tq in _context.TblQuizzes
                            where !_context.DisableLists.Any(x => x.DisableId == tq.QuizId && x.UserId == id)
                            select tq;
            return View();
        }

        public IActionResult NotifyDetails(int id)
        {
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
