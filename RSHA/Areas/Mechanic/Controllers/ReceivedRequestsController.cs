using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSHA.Data;
using RSHA.Models;
using RSHA.Models.ViewModels;

namespace RSHA.Areas.Mechanic.Controllers
{
    [Area("Mechanic")]
    public class ReceivedRequestsController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public RequestsViewModel RequestsVM { get; set; }

        public ReceivedRequestsController(ApplicationDbContext db)
        {
            _db = db;
            RequestsVM = new RequestsViewModel()
            {
                ProblemTypes = _db.ProblemTypes.ToList(),
                Requests = new Models.Requests()
            };
        }

        // INDEX Action Method      --------------------------------    INDEX
        public async Task<IActionResult> Index()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var mechanic = await _db.Mechanics.FirstOrDefaultAsync(u => u.UserId == id);
            //var mechanic = _db.Mechanics.Where(u => u.UserId == id);

            //var requests = await _db.Requests.FirstOrDefaultAsync(m => m.MechanicAssigned == m.Id);
            var requests = _db.Requests.Where(m => m.MechanicAssigned == mechanic.Id).Include(m => m.ProblemTypes);

            return View(await requests.ToListAsync());
        }

        // ACCEPT Action Method      --------------------------------    ACCEPT
        public async Task<IActionResult> Accept()
        {
            return View();


            //need to be put in a list or somehting, and possbile have stages of progress. Also comment from mech?
        }






        // DELETE Action Method      --------------------------------    DELETE
        public async Task<IActionResult> Delete()
        {
            return View();

            //just delete request and send notification and email to customer notifying them of cannon accept this request
        }

    }
}