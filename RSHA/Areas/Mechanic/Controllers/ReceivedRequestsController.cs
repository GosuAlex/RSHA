using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSHA.Data;

namespace RSHA.Areas.Mechanic.Controllers
{
    [Area("Mechanic")]
    public class ReceivedRequestsController : Controller
    {
        //private readonly ApplicationDbContext _db;

        //[BindProperty]
        //public RequestsViewModel RequestsVM { get; set; }

        //public ReceivedRequestsController(ApplicationDbContext db)
        //{
        //    _db = db;
        //    RequestsVM = new RequestsViewModel()
        //    {
        //        ProblemTypes = _db.ProblemTypes.ToList(),
        //        Requests = new Models.Requests()
        //    };
        //}

        //// INDEX Action Method      --------------------------------    INDEX
        //public async Task<IActionResult> Index()
        //{
        //    var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var requests = _db.Requests.Where(u => u.CustomerId == id).Include(m => m.ProblemTypes);

        //    return View(await requests.ToListAsync());
        //}
    }
}