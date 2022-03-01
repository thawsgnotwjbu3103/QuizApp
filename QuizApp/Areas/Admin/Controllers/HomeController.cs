using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly testContext _context;
        public HomeController(testContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Users = _context.UserInfos.Count();
            ViewBag.Quizs = _context.TblQuizzes.Count();
            return View();
        }
    }
}
