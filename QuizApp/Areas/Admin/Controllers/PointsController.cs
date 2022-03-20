using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PointsController : Controller
    {
        private readonly testContext _context;

        public PointsController(testContext context)
        {
            _context = context;
        }

        // GET: Admin/Points
        public async Task<IActionResult> Index(int id)
        {
            var testContext = _context.Points.Where(x => x.UserId == id);
            var temp = testContext.Select(x => x.QuizId).FirstOrDefault();
            ViewBag.Name = _context.TblQuizzes.Where(x => x.QuizId == temp).Select(x => x.QuizName).FirstOrDefault();
            return View(await testContext.ToListAsync());
        }
        private bool PointExists(int id)
        {
            return _context.Points.Any(e => e.PointId == id);
        }
    }
}
