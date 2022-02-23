using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly testContext _context;
        public QuizController(testContext context)
        {
            _context = context;
        }
        public IActionResult Start(int id)
        {
            if (Request.Cookies["_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            var check = _context.Users
                .Where(x => x.UserId == int.Parse(Request.Cookies["_id"]))
                .FirstOrDefault();
            if (check == null) return RedirectToAction("Index", "Home", new { area = "" });

            var question = _context.Questions.Include(q => q.Quiz).Where(x => x.QuizId == id).ToList();

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(182.5);
            Response.Cookies.Append("_quizId", id.ToString(), options);
            var questionChoices = _context.QuestionChoices
                .Join(_context.Questions,
                qc => qc.Question.QuestionId,
                q => q.QuestionId,
                (qc, q) => new QuestionChoice
                {
                    QuestionId = qc.QuestionId,
                    ChoiceId = qc.ChoiceId,
                    Choice = qc.Choice
                }).ToList();
            ViewBag.QuestionChoices = questionChoices;
            ViewBag.Questions = question;
            ViewBag.Count = question.Count();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostQuiz(int[] answerList, [Bind("UserAnswerId,UserId,QuizId,QuestionId,ChoiceId,IsRight")] UserAnswer userAnswer)
        {
            List<UserAnswer> userAnswers = new List<UserAnswer>();
            foreach(var item in answerList)
            {
                UserAnswer ua = new UserAnswer
                {
                    UserId = int.Parse(Request.Cookies["_id"]),
                    QuizId = int.Parse(Request.Cookies["_quizId"]),
                    QuestionId = _context.QuestionChoices.Where(x=>x.ChoiceId == item).Select(q=>q.QuestionId).FirstOrDefault(),
                    ChoiceId = item,
                    IsRight = _context.QuestionChoices.Where(x => x.ChoiceId == item).Select(q => q.IsRight).FirstOrDefault()
                };
                userAnswers.Add(ua);
            };
            _context.UserAnswers.AddRange(userAnswers);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard", new { area = "" });
        }
    }
}
