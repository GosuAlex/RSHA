using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSHA.Data;
using RSHA.Models;

namespace RSHA.Areas.Mechanic.Controllers
{
    [Area("Mechanic")]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Feedbacks Feedback { get; set; }

        public FeedbackController(ApplicationDbContext db)
        {
            _db = db;
            Feedback = new Models.Feedbacks();
        }

        // INDEX Action Method      --------------------------------    INDEX
        public async Task<IActionResult> Index()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var feedbacks = _db.Feedbacks.Where(f => f.UserId == id).OrderBy(p => EF.Property<object>(p, sortColumn));
            var feedback = _db.Feedbacks.Where(f => f.UserId == id);

            //return View(await feedback.ToListAsync());
            return View();
        }

        // POST Create Action Method
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            Feedback.FeedbackCreated = DateTime.Now;

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Feedback.UserId = id;
            Feedback.SenderName = User.Identity.Name;

            _db.Feedbacks.Add(Feedback);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}