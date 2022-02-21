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
    public class TblQuizsController : Controller
    {
        private readonly testContext _context;
        public INotyfService _notifyService { get; }
        public TblQuizsController(testContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/TblQuizs
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblQuizzes.ToListAsync());
        }
        // GET: Admin/TblQuizs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TblQuizs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizId,QuizName,Time,DateCreated,IsActive")] TblQuiz tblQuiz)
        {
            if (ModelState.IsValid)
            {
                tblQuiz.DateCreated = DateTime.Now.ToString("dd-MM-yyyy");
                _context.Add(tblQuiz);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(tblQuiz);
        }

        // GET: Admin/TblQuizs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tblQuiz = await _context.TblQuizzes.FindAsync(id);
            if (tblQuiz == null)
            {
                return NotFound();
            }
            return View(tblQuiz);
        }

        // POST: Admin/TblQuizs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizId,QuizName,Time,DateCreated,IsActive")] TblQuiz tblQuiz)
        {
            if (id != tblQuiz.QuizId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tblQuiz.DateCreated = DateTime.Now.ToString("dd-MM-yyyy");           
                    _context.Update(tblQuiz);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Sửa thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblQuizExists(tblQuiz.QuizId))
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
            return View(tblQuiz);
        }
        private bool TblQuizExists(int id)
        {
            return _context.TblQuizzes.Any(e => e.QuizId == id);
        }
    }
}
