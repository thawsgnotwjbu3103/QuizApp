using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuizApp.Models;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
            ViewBag.QuizId = new SelectList(_context.TblQuizzes, "QuizId", "QuizName");
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

        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || Path.GetExtension(file.FileName) != ".xlsx")
            {
                _notifyService.Error("Không có file nào được chọn");
                return RedirectToAction(nameof(Index));
            };
            var list = new List<TblQuiz>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    if (rowcount <= 0)
                    {
                        _notifyService.Error("File excel không có dữ liệu bên trong");
                        return RedirectToAction(nameof(Index));
                    }
                    for (int row = 2; row <= rowcount; row++)
                    {
                        list.Add(new TblQuiz
                        {
                            QuizName = worksheet.Cells[row, 1].Value.ToString(),
                            Time = worksheet.Cells[row, 2].Value.ToString(),
                            IsActive = Convert.ToBoolean(worksheet.Cells[row, 3].Value),
                            DateCreated = Convert.ToDateTime(worksheet.Cells[row, 4].Value).ToString("dd-MM-yyyy")
                        });
                    }
                }
            }
            try
            {
                _context.TblQuizzes.AddRange(list);
                await _context.SaveChangesAsync();
                _notifyService.Success("Import thành công");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ImportQuestion(IFormFile file, int quizId)
        {
            ViewBag.QuizId = new SelectList(_context.TblQuizzes, "QuizId", "QuizName");
            if (file == null || Path.GetExtension(file.FileName) != ".xlsx")
            {
                _notifyService.Error("Không có file nào được chọn");
                return RedirectToAction(nameof(Index));
            };
            var questionList = new List<Question>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    if (rowcount <= 0)
                    {
                        _notifyService.Error("File excel không có dữ liệu bên trong");
                        return RedirectToAction(nameof(Index));
                    }

                    for (int row = 2; row <= rowcount; row++)
                    {
                        questionList.Add(new Question
                        {
                            QuizId = quizId,
                            QuestionTitle = worksheet.Cells[row, 1].Value.ToString(),
                            IsMultipleChoices = Convert.ToBoolean(worksheet.Cells[row, 2].Value)
                        });
                    }
                }
            }
            try
            {
                _context.Questions.AddRange(questionList);
                await _context.SaveChangesAsync();
                _notifyService.Success("Import thành công");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ImportChoices(IFormFile file, int quizId)
        {
            ViewBag.QuizId = new SelectList(_context.TblQuizzes, "QuizId", "QuizName");
            if (file == null || Path.GetExtension(file.FileName) != ".xlsx")
            {
                _notifyService.Error("Không có file nào được chọn");
                return RedirectToAction(nameof(Index));
            };
            var choiceList = new List<QuestionChoice>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    if (rowcount <= 0)
                    {
                        _notifyService.Error("File excel không có dữ liệu bên trong");
                        return RedirectToAction(nameof(Index));
                    }

                    for (int row = 2; row <= rowcount; row++)
                    {
                        choiceList.Add(new QuestionChoice
                        {
                            QuizId = quizId,
                            QuestionId = _context.Questions
                            .Where(x=>x.QuestionTitle == worksheet.Cells[row, 1].Value.ToString())
                            .Select(x=>x.QuestionId).First(),
                            Choice = worksheet.Cells[row, 2].Value.ToString(),
                            IsRight = Convert.ToBoolean(worksheet.Cells[row, 3].Value)
                        });
                    }
                }
            }
            try
            {
                _context.QuestionChoices.AddRange(choiceList);
                await _context.SaveChangesAsync();
                _notifyService.Success("Import thành công");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }


        private bool TblQuizExists(int id)
        {
            return _context.TblQuizzes.Any(e => e.QuizId == id);
        }
    }
}
