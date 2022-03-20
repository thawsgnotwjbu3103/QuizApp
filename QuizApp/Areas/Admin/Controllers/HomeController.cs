using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly testContext _context;
        public HomeController(testContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var userTests = (from u in _context.UserInfos
                             join ua in _context.UserAnswers on u.UserId equals ua.UserId
                             join q in _context.TblQuizzes on ua.QuizId equals q.QuizId
                             select new UserTests
                             {
                                 UserId = u.UserId,
                                 QuizId = q.QuizId,
                                 QuizName = q.QuizName,
                                 FullName = u.FullName
                             }).Distinct().ToList();
            ViewBag.UserTests = userTests;
            ViewBag.Users = _context.UserInfos.Count();
            ViewBag.Quizs = _context.TblQuizzes.Count();
            return View();
        }

        public IActionResult Details(int uid, int qid)
        {
            var point = _context.Points.Where(x => x.UserId == uid && x.QuizId == qid).Select(x => x.TotalPoint).FirstOrDefault();
            var text = _context.UserAnswerTexts.Include(x => x.QuestionText).Where(x => x.UserId == uid && x.QuestionText.QuizId == qid).ToList();
            var user = _context.UserInfos.Where(x => x.UserId == uid).ToList();
            var quiz = _context.TblQuizzes.Where(x => x.QuizId == qid).ToList();

            ViewBag.Point = point;
            ViewBag.Text = text;
            ViewBag.User = user;
            ViewBag.Quiz = quiz;
            return View();
        }

        public IActionResult Export()
        {
            var list = (from u in _context.UserInfos
                        join ua in _context.UserAnswers on u.UserId equals ua.UserId
                        join q in _context.TblQuizzes on ua.QuizId equals q.QuizId
                        select new
                        {
                            UserId = u.UserId,
                            QuizId = q.QuizId,
                            QuizName = q.QuizName,
                            FullName = u.FullName
                        }).Distinct().ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Ho va ten";
                worksheet.Cell(currentRow, 2).Value = "Gioi tinh";
                worksheet.Cell(currentRow, 3).Value = "Ngay sinh";
                worksheet.Cell(currentRow, 4).Value = "So CMND/CCCD";
                worksheet.Cell(currentRow, 5).Value = "So dien thoai";
                worksheet.Cell(currentRow, 6).Value = "Dia chi";
                worksheet.Cell(currentRow, 7).Value = "Email";
                worksheet.Cell(currentRow, 8).Value = "Ngay dang ky";
                worksheet.Cell(currentRow, 9).Value = "Bai thi";
                worksheet.Cell(currentRow, 10).Value = "Thoi gian";
                worksheet.Cell(currentRow, 11).Value = "Tong diem";
                worksheet.Cell(currentRow, 12).Value = "Cau hoi tu luan";
                worksheet.Cell(currentRow, 13).Value = "Tra loi tu luan";


                foreach (var item in list)
                {
                    var user = _context.UserInfos.Where(x => x.UserId == item.UserId).FirstOrDefault();
                    var quiz = _context.TblQuizzes.Where(x => x.QuizId == item.QuizId).FirstOrDefault();
                    var point = _context.Points.Where(x => x.QuizId == item.QuizId && x.UserId == item.UserId).FirstOrDefault();
                    var text = _context.UserAnswerTexts.Include(x => x.QuestionText).Where(x => x.UserId == item.UserId && x.QuestionText.QuizId == item.QuizId).ToList();

                    currentRow++;
                    foreach (var uat in text)
                    {
                        worksheet.Cell(currentRow, 1).Value = item.FullName;
                        worksheet.Cell(currentRow, 2).Value = user.Gender;
                        worksheet.Cell(currentRow, 3).Value = user.Birthday;
                        worksheet.Cell(currentRow, 4).Value = user.IdNum;
                        worksheet.Cell(currentRow, 5).Value = "'" + user.PhoneNum;
                        worksheet.Cell(currentRow, 6).Value = user.Address;
                        worksheet.Cell(currentRow, 7).Value = user.Email;
                        worksheet.Cell(currentRow, 8).Value = user.DateCreated;
                        worksheet.Cell(currentRow, 9).Value = item.QuizName;
                        worksheet.Cell(currentRow, 10).Value = quiz.Time;
                        worksheet.Cell(currentRow, 11).Value = point.TotalPoint;
                        worksheet.Cell(currentRow, 12).Value = uat.QuestionText.QuestionTextTitle;
                        worksheet.Cell(currentRow, 13).Value = uat.QuestionTextTitle;
                    };
                };


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var fileName = "DSTS-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
