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
    public class QuestionChoicesController : Controller
    {
        private readonly testContext _context;

        public QuestionChoicesController(testContext context)
        {
            _context = context;
        }

        // GET: Admin/QuestionChoices
        public async Task<IActionResult> Index(int id)
        {
            var testContext = _context.QuestionChoices.Include(q => q.Question).Where(x=>x.QuestionId == id);
            return View(await testContext.ToListAsync());
        }

        // GET: Admin/QuestionChoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionChoice = await _context.QuestionChoices
                .Include(q => q.Question)
                .FirstOrDefaultAsync(m => m.ChoiceId == id);
            if (questionChoice == null)
            {
                return NotFound();
            }

            return View(questionChoice);
        }

        // GET: Admin/QuestionChoices/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "QuestionTitle");
            return View();
        }

        // POST: Admin/QuestionChoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChoiceId,QuestionId,IsRight,Choice")] QuestionChoice questionChoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionChoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "QuestionTitle", questionChoice.QuestionId);
            return View(questionChoice);
        }

        // GET: Admin/QuestionChoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("ChoiceId,QuestionId,IsRight,Choice")] QuestionChoice questionChoice)
        {
            if (id != questionChoice.ChoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "QuestionTitle", questionChoice.QuestionId);
            return View(questionChoice);
        }

        // GET: Admin/QuestionChoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionChoice = await _context.QuestionChoices
                .Include(q => q.Question)
                .FirstOrDefaultAsync(m => m.ChoiceId == id);
            if (questionChoice == null)
            {
                return NotFound();
            }

            return View(questionChoice);
        }

        // POST: Admin/QuestionChoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionChoice = await _context.QuestionChoices.FindAsync(id);
            _context.QuestionChoices.Remove(questionChoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionChoiceExists(int id)
        {
            return _context.QuestionChoices.Any(e => e.ChoiceId == id);
        }
    }
}
