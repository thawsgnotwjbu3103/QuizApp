using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuestionTextsController : Controller
    {
        private readonly testContext _context;
        public INotyfService _notifyService { get; }
        public QuestionTextsController(testContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/QuestionTexts
        public async Task<IActionResult> Index(int id)
        {
            var testContext = _context.QuestionTexts.Include(q => q.Quiz).Where(x => x.QuizId == id);
            ViewBag.qId = id;
            return View(await testContext.ToListAsync());
        }

        // GET: Admin/QuestionTexts/Create
        public IActionResult Create(int id)
        {
            ViewBag.qId = id;
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated");
            return View();
        }

        // POST: Admin/QuestionTexts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("QuestionTextId,QuestionTextTitle,QuizId")] QuestionText questionsText)
        {

            if (ModelState.IsValid)
            {

                questionsText.QuizId = id;
                _context.Add(questionsText);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo thành công");
                return RedirectToAction(nameof(Index), new { id = id });
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated", questionsText.QuizId);
            return View(questionsText);
        }

        // GET: Admin/QuestionTexts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.qId = _context.QuestionTexts.Where(x => x.QuestionTextId == id).Select(q => q.QuizId).FirstOrDefault();
            if (id == null)
            {
                return NotFound();
            }

            var questionsText = await _context.QuestionTexts.FindAsync(id);
            if (questionsText == null)
            {
                return NotFound();
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated", questionsText.QuizId);
            return View(questionsText);
        }

        // POST: Admin/QuestionTexts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionTextId,QuestionTextTitle,QuizId")] QuestionText questionsText)
        {
            var quizId = _context.QuestionTexts.Where(x => x.QuestionTextId == id).Select(q => q.QuizId).FirstOrDefault();
            ViewBag.qId = quizId;
            if (id != questionsText.QuestionTextId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    questionsText.QuizId = quizId;
                    _context.Update(questionsText);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsTextExists(questionsText.QuestionTextId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = quizId });
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated", questionsText.QuizId);
            return View(questionsText);
        }

        private bool QuestionsTextExists(int id)
        {
            return _context.QuestionTexts.Any(e => e.QuestionTextId == id);
        }
    }
}
