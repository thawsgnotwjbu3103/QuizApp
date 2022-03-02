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
    public class QuestionChoicesController : Controller
    {
        private readonly testContext _context;
        public INotyfService _notifyService { get; }
        public QuestionChoicesController(testContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/QuestionChoices
        public async Task<IActionResult> Index(int id)
        {
            var testContext = _context.QuestionChoices
                .Include(q => q.Question)
                .Where(x=>x.QuestionId == id);


            ViewBag.qId = _context.QuestionChoices
                .Join(_context.Questions,
                      qc => qc.QuestionId,
                      q => q.QuestionId,
                      (qc, q) => new
                      {
                          q.QuizId,
                          q.QuestionId
                      })
                .Where(x=>x.QuestionId == id)
                .Select(x=>x.QuizId).FirstOrDefault();

            ViewBag.Id = id;
            return View(await testContext.ToListAsync());
        }

        // GET: Admin/QuestionChoices/Create
        public IActionResult Create(int id)
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "QuestionTitle");
            ViewBag.Id = id;
            return View();
        }

        // POST: Admin/QuestionChoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ChoiceId,QuestionId,QuizId,IsRight,Choice")] QuestionChoice questionChoice)
        {
            var check = (from qc in _context.QuestionChoices
                         join q in _context.Questions on qc.QuestionId equals q.QuestionId
                         where q.QuestionId == id && 
                         q.IsMultipleChoices == false && 
                         qc.IsRight == true
                         select new
                         {
                             temp = qc.IsRight
                         }).Count();
            var quizId = _context.Questions.Where(x => x.QuestionId == id).Select(q => q.QuizId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if(questionChoice.IsRight == true && check == 1)
                {
                    _notifyService.Warning("Không thể có hai câu trả lời đúng trong câu hỏi có dạng duy nhất");
                    return View(questionChoice);
                };
                questionChoice.QuestionId = id;
                questionChoice.QuizId = quizId;
                _context.Add(questionChoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = id});
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "QuestionTitle", questionChoice.QuestionId);
            return View(questionChoice);
        }

        // GET: Admin/QuestionChoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.qId = _context.QuestionChoices.Where(x => x.ChoiceId == id).Select(m => m.QuestionId).FirstOrDefault();
            if (id == null)
            {
                return NotFound();
            }

            var questionChoice = await _context.QuestionChoices.FindAsync(id);
            if (questionChoice == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "QuestionTitle", questionChoice.QuestionId);
            return View(questionChoice);
        }

        // POST: Admin/QuestionChoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChoiceId,QuestionId,QuizId,IsRight,Choice")] QuestionChoice questionChoice)
        {
            var oldId = _context.QuestionChoices.Where(x => x.ChoiceId == id).AsNoTracking().FirstOrDefault();
            var check = (from qc in _context.QuestionChoices
                         join q in _context.Questions on qc.QuestionId equals q.QuestionId
                         where q.QuestionId == oldId.QuestionId &&
                         q.IsMultipleChoices == false &&
                         qc.IsRight == true
                         select new
                         {
                             temp = qc.IsRight
                         }).Count();
            ViewBag.qId = oldId.QuestionId;
            if (id != questionChoice.ChoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (questionChoice.IsRight == true && check == 1)
                    {
                        _notifyService.Warning("Không thể có hai câu trả lời đúng trong câu hỏi có dạng duy nhất");
                        return View(questionChoice);
                    };
                    questionChoice.QuestionId = oldId.QuestionId;
                    questionChoice.QuizId = oldId.QuizId;
                    _context.Update(questionChoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionChoiceExists(questionChoice.ChoiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = ViewBag.qId });
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "QuestionTitle", questionChoice.QuestionId);
            return View(questionChoice);
        }

        // GET: Admin/QuestionChoices/Delete/5
        private bool QuestionChoiceExists(int id)
        {
            return _context.QuestionChoices.Any(e => e.ChoiceId == id);
        }
    }
}
