using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserInfoesController : Controller
    {
        private readonly testContext _context;

        public UserInfoesController(testContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.UserInfos.ToListAsync());
        }

        public IActionResult Export()
        {
            var list = _context.UserInfos.ToList();
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


                foreach (var item in list)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.FullName;
                    worksheet.Cell(currentRow, 2).Value = item.Gender;
                    worksheet.Cell(currentRow, 3).Value = item.Birthday;
                    worksheet.Cell(currentRow, 4).Value = item.IdNum;
                    worksheet.Cell(currentRow, 5).Value = "'" + item.PhoneNum;
                    worksheet.Cell(currentRow, 6).Value = item.Address;
                    worksheet.Cell(currentRow, 7).Value = item.Email;
                    worksheet.Cell(currentRow, 8).Value = item.DateCreated;
                };


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var fileName = "DSTTCN-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private bool UserExists(int id)
        {
            return _context.UserInfos.Any(e => e.UserId == id);
        }
    }
}
