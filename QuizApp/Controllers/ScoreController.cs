using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using System.Linq;

namespace QuizApp.Controllers
{
    public class ScoreController : Controller
    {
        private readonly testContext _context;
        public ScoreController(testContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id, int quizId)
        {
            var userAnswerTotal = _context.UserAnswers
                .Where(x => x.UserId == id && x.QuizId == quizId && x.ChoiceId != null && x.IsRight == true).Count();
            var title = _context.TblQuizzes.Where(x => x.QuizId == quizId).Select(q => q.QuizName).FirstOrDefault();

            var totalQuestions = _context.QuestionChoices.Where(x => x.IsRight == true && x.QuizId == quizId).Count();

            var point = _context.Points.Where(x => x.UserId == id && x.QuizId == quizId).Select(q => q.TotalPoint).FirstOrDefault();

            ViewBag.Total = totalQuestions;
            ViewBag.QuizTitle = title;
            ViewBag.Point = userAnswerTotal;
            ViewBag.TotalPoint = point;
            return View();
        }
    }
}
