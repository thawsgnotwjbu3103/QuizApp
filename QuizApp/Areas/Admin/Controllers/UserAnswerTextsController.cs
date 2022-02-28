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

        // GET: Admin/UserAnswerTexts
        public async Task<IActionResult> Index()
        {
            var testContext = _context.UserAnswerTexts.Include(u => u.QuestionText).Include(u => u.User);
            return View(await testContext.ToListAsync());
        }

        // GET: Admin/UserAnswerTexts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAnswerText = await _context.UserAnswerTexts
                .Include(u => u.QuestionText)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UaTextId == id);
            if (userAnswerText == null)
            {
                return NotFound();
            }

            return View(userAnswerText);
        }

        // GET: Admin/UserAnswerTexts/Create
        public IActionResult Create()
        {
            ViewData["QuestionTextId"] = new SelectList(_context.QuestionTexts, "QuestionTextId", "QuestionTextTitle");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Address");
            return View();
        }

        // POST: Admin/UserAnswerTexts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UaTextId,QuestionTextId,UserId,QuestionTextTitle,Matches")] UserAnswerText userAnswerText)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAnswerText);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionTextId"] = new SelectList(_context.QuestionTexts, "QuestionTextId", "QuestionTextTitle", userAnswerText.QuestionTextId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Address", userAnswerText.UserId);
            return View(userAnswerText);
        }

        // GET: Admin/UserAnswerTexts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAnswerText = await _context.UserAnswerTexts.FindAsync(id);
            if (userAnswerText == null)
            {
                return NotFound();
            }
            ViewData["QuestionTextId"] = new SelectList(_context.QuestionTexts, "QuestionTextId", "QuestionTextTitle", userAnswerText.QuestionTextId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Address", userAnswerText.UserId);
            return View(userAnswerText);
        }

        // POST: Admin/UserAnswerTexts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UaTextId,QuestionTextId,UserId,QuestionTextTitle,Matches")] UserAnswerText userAnswerText)
        {
            if (id != userAnswerText.UaTextId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAnswerText);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAnswerTextExists(userAnswerText.UaTextId))
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
            ViewData["QuestionTextId"] = new SelectList(_context.QuestionTexts, "QuestionTextId", "QuestionTextTitle", userAnswerText.QuestionTextId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Address", userAnswerText.UserId);
            return View(userAnswerText);
        }

        // GET: Admin/UserAnswerTexts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAnswerText = await _context.UserAnswerTexts
                .Include(u => u.QuestionText)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UaTextId == id);
            if (userAnswerText == null)
            {
                return NotFound();
            }

            return View(userAnswerText);
        }

        // POST: Admin/UserAnswerTexts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAnswerText = await _context.UserAnswerTexts.FindAsync(id);
            _context.UserAnswerTexts.Remove(userAnswerText);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAnswerTextExists(int id)
        {
            return _context.UserAnswerTexts.Any(e => e.UaTextId == id);
        }
    }
}
