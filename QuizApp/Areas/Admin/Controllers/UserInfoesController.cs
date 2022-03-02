using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

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

        private bool UserExists(int id)
        {
            return _context.UserInfos.Any(e => e.UserId == id);
        }
    }
}
