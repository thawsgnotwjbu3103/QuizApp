using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserAnswerTextsController : Controller
    {
        private readonly testContext _context;

        public UserAnswerTextsController(testContext context)
        {
            _context = context;
        }

        // GET: Admin/UserAnswerTexts1
        public async Task<IActionResult> Index(int id)
        {
            var testContext = _context.UserAnswerTexts.Include(u => u.QuestionText).Include(u => u.User).Where(x=>x.UserId == id);
            return View(await testContext.ToListAsync());
        }

        public IActionResult Details(int id)
        {
            var userAnswerText = _context.UserAnswerTexts
                .Include(u => u.QuestionText)
                .Include(u => u.User).Where(x => x.QuestionTextId == id)
                .FirstOrDefault();
            return View(userAnswerText);
        }
    }
}
