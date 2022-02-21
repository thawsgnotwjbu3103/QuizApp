using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionsController : Controller
    {
        private readonly testContext _context;
        public INotyfService _notifyService { get; }
        public QuestionsController(testContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/Questions
        public async Task<IActionResult> Index(int id)
        {
            var testContext = _context.Questions.Include(q => q.Quiz).Where(x=>x.QuizId == id);
            ViewBag.Id = id;
            return View(await testContext.ToListAsync());
        }
        // GET: Admin/Questions/Create
        public IActionResult Create(int id)
        {
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "QuizName");
            ViewBag.Id = id;
            return View();
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("QuestionId,QuizId,QuestionTitle,IsMultipleChoices")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.QuizId = id;
                _context.Add(question);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo thành công");
                return RedirectToAction(nameof(Index), new { id = id});
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "QuizName", question.QuizId);
            return View(question);
        }

        // GET: Admin/Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.qId = _context.Questions.Where(x => x.QuestionId == id).Select(m => m.QuizId).FirstOrDefault();
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "QuizName", question.QuizId);
            return View(question);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,QuizId,QuestionTitle,IsMultipleChoices")] Question question)
        {
            var oldId = _context.Questions.Where(x => x.QuestionId == id).Select(m => m.QuizId).FirstOrDefault();
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    question.QuizId = oldId;
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = oldId});
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "QuizName", question.QuizId);
            return View(question);
        }
        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}
