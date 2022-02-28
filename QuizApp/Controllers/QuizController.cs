using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var questionText = _context.QuestionTexts.Include(q => q.Quiz).Where(x => x.QuizId == id).ToList();

            CookieOptions options = new CookieOptions();
            options.Expires = new DateTimeOffset(2038, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));
            Response.Cookies.Append("_quizId", id.ToString(), options);

            string time = _context.TblQuizzes.Where(x => x.QuizId == id).Select(q => q.Time).FirstOrDefault();
            int intTime = Int32.Parse(string.Join(string.Empty, Regex.Matches(time, @"\d+").OfType<Match>().Select(m => m.Value)));
            ViewBag.Time = intTime;

            var questionChoices = (from qc in _context.QuestionChoices
                                   join q in _context.Questions on qc.QuestionId equals q.QuestionId
                                   join qz in _context.TblQuizzes on q.QuizId equals qz.QuizId
                                   select new QuestionChoice
                                   {
                                       QuizId = qz.QuizId,
                                       QuestionId = qc.QuestionId,
                                       ChoiceId = qc.ChoiceId,
                                       Choice = qc.Choice
                                   }).Where(x=>x.QuizId == id).ToList();


            ViewBag.QuestionChoices = questionChoices;
            ViewBag.Questions = question;
            ViewBag.Count = question.Count();
            ViewBag.QuestionText = questionText;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostQuiz(int qtId, int matches, string[] textanswerList, int[] answerList,
            [Bind("UserAnswerId,UserId,QuizId,QuestionId,ChoiceId,IsRight")] UserAnswer userAnswer,
            [Bind("uaTextId,QuestionTextId,UserId,QuestionTextTitle,Matches")] UserAnswerText answerText)
        {
            List<UserAnswer> userAnswers = new List<UserAnswer>();
            List<UserAnswerText> userAnswerTexts = new List<UserAnswerText>();

            foreach (var item in answerList)
            {
                UserAnswer ua = new UserAnswer
                {
                    UserId = int.Parse(Request.Cookies["_id"]),
                    QuizId = int.Parse(Request.Cookies["_quizId"]),
                    QuestionId = _context.QuestionChoices.Where(x => x.ChoiceId == item).Select(q => q.QuestionId).FirstOrDefault(),
                    ChoiceId = item,
                    IsRight = _context.QuestionChoices.Where(x => x.ChoiceId == item).Select(q => q.IsRight).FirstOrDefault()
                };
                userAnswers.Add(ua);
            };

            foreach (var item in textanswerList)
            {
                UserAnswerText uat = new UserAnswerText
                {
                    UserId = int.Parse(Request.Cookies["_id"]),
                    QuestionTextId = qtId,
                    Matches = matches.ToString(),
                    QuestionTextTitle = item
                };
                userAnswerTexts.Add(uat);
            }

            _context.UserAnswerTexts.AddRange(userAnswerTexts);
            _context.UserAnswers.AddRange(userAnswers);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard", new { area = "" });
        }
    }
}
