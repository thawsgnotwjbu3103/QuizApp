using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class NotificationsController : Controller
    {
        private readonly testContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notifyService { get; }
        public NotificationsController(testContext context,
            IWebHostEnvironment webHostEnvironment, INotyfService notifyService)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _notifyService = notifyService;
        }
        // GET: Admin/Notifications
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notifications.ToListAsync());
        }

        // GET: Admin/Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.NotifyId == id);
            ViewBag.Content = notification.NotifyContent;

            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Admin/Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NotifyId,Title,NotifyContent,DateCreated,DateUpdated,IsActive,Content")] Models.Notification notification)
        {
            if (ModelState.IsValid)
            {
                //string content = "";
                //if (notification.Content != null)
                //{
                //    string uploadDirs = Path.Combine(_webHostEnvironment.WebRootPath, "contentfiles");
                //    content = Guid.NewGuid().ToString() + "_" + notification.Content.FileName;
                //    string filePath = Path.Combine(uploadDirs, content);
                //    FileStream fs = new FileStream(filePath, FileMode.Create);
                //    await notification.Content.CopyToAsync(fs);
                //    fs.Close();
                //}
                //notification.NotifyContent = content;
                //notification.DateCreated = DateTime.Now.ToString("dd-MM-yyyy");
                //notification.DateUpdated = DateTime.Now.ToString("dd-MM-yyyy");


                _context.Add(notification);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Admin/Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Admin/Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NotifyId,Title,NotifyContent,DateCreated,DateUpdated,IsActive,Content")] Models.Notification notification)
        {
            var oldInfo = _context.Notifications.AsNoTracking().FirstOrDefault(x => x.NotifyId == id);
            if (id != notification.NotifyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                notification.NotifyContent = oldInfo.NotifyContent;
                notification.Title = oldInfo.Title;
                notification.DateCreated = oldInfo.DateCreated;
                notification.DateUpdated = DateTime.Now.ToString("dd-MM-yyyy");
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Đổi trạng thái thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.NotifyId))
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
            return View(notification);
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.NotifyId == id);
        }
    }
}
