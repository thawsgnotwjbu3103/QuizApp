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
            var userTests = (from u in _context.UserInfos
                                 join ua in _context.UserAnswers on u.UserId equals ua.UserId
                                 join q in _context.TblQuizzes on ua.QuizId equals q.QuizId
                                 select new UserTests
                                 {
                                     UserId = u.UserId,
                                     QuizId = q.QuizId,
                                     QuizName = q.QuizName,
                                     FullName = u.FullName
                                 }).Distinct().ToList();
            ViewBag.UserTests = userTests;
            ViewBag.Users = _context.UserInfos.Count();
            ViewBag.Quizs = _context.TblQuizzes.Count();
            return View();
        }

        public IActionResult Details(int uid, int qid)
        {
            var point = _context.Points.Where(x => x.UserId == uid && x.QuizId == qid).Select(x => x.TotalPoint).FirstOrDefault();
            var text = _context.UserAnswerTexts.Where(x => x.UserId == uid && x.QuestionText.QuizId == qid).Select(x=>x.QuestionTextTitle).FirstOrDefault();
            var user = _context.UserInfos.Where(x => x.UserId == uid).ToList();
            var quiz = _context.TblQuizzes.Where(x=>x.QuizId == qid).ToList();

            ViewBag.Point = point;
            ViewBag.Text = text;
            ViewBag.User = user;
            ViewBag.Quiz = quiz;
            return View();
        }
    }
}
