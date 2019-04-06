using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSHA.Data;
using RSHA.Models;

namespace RSHA.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RequestsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Requests.ToList());
        }

        // GET Create Action Method --------------------------------    CREATE
        public IActionResult Create()
        {
            return View();
        }

        // POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Requests requests)
        {
            if (ModelState.IsValid)
            {
                _db.Add(requests);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requests);
        }

        // GET Edit Action Method   --------------------------------    EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _db.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Requests requests)
        {
            if (id != requests.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(requests);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requests);
        }

        // GET Details Action Method    --------------------------------    DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requests = await _db.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // GET Delete Action Method --------------------------------        DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requests = await _db.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // POST Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var productTypes = await _db.Requests.FindAsync(id);
            _db.Requests.Remove(productTypes);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}