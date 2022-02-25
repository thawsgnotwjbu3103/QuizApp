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
    public class QuestionsTextsController : Controller
    {
        private readonly testContext _context;
        public INotyfService _notifyService { get; }
        public QuestionsTextsController(testContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/QuestionsTexts
        public async Task<IActionResult> Index(int id)
        {
            var testContext = _context.QuestionsTexts.Include(q => q.Quiz).Where(x=>x.QuizId == id);
            ViewBag.qId = id;
            return View(await testContext.ToListAsync());
        }

        // GET: Admin/QuestionsTexts/Create
        public IActionResult Create(int id)
        {
            ViewBag.qId = id;
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated");
            return View();
        }

        // POST: Admin/QuestionsTexts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("QuestionTextId,QuestionTextTitle,QuizId")] QuestionsText questionsText)
        {
            
            if (ModelState.IsValid)
            {

                questionsText.QuizId = id;
                _context.Add(questionsText);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo thành công");
                return RedirectToAction(nameof(Index),new { id = id});
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated", questionsText.QuizId);
            return View(questionsText);
        }

        // GET: Admin/QuestionsTexts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.qId = _context.QuestionsTexts.Where(x => x.QuestionTextId == id).Select(q => q.QuizId).FirstOrDefault();
            if (id == null)
            {
                return NotFound();
            }

            var questionsText = await _context.QuestionsTexts.FindAsync(id);
            if (questionsText == null)
            {
                return NotFound();
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated", questionsText.QuizId);
            return View(questionsText);
        }

        // POST: Admin/QuestionsTexts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionTextId,QuestionTextTitle,QuizId")] QuestionsText questionsText)
        {
            var quizId = _context.QuestionsTexts.Where(x => x.QuestionTextId == id).Select(q => q.QuizId).FirstOrDefault();
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
                return RedirectToAction(nameof(Index), new {id =  quizId});
            }
            ViewData["QuizId"] = new SelectList(_context.TblQuizzes, "QuizId", "DateCreated", questionsText.QuizId);
            return View(questionsText);
        }

        private bool QuestionsTextExists(int id)
        {
            return _context.QuestionsTexts.Any(e => e.QuestionTextId == id);
        }
    }
}
