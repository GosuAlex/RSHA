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
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace RSHA.Areas.Mechanic.Controllers
{
    [Area("Mechanic")]
    public class ReceivedRequestsController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IConfiguration _configuration { get; }

        [BindProperty]
        public RequestsViewModel RequestsVM { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public ReceivedRequestsController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _db = db;
            RequestsVM = new RequestsViewModel()
            {
                ProblemTypes = _db.ProblemTypes.ToList(),
                Requests = new Models.Requests()
            };
            _userManager = userManager;
            _configuration = configuration;
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

        // GET ACCEPT Action Method      --------------------------------    ACCEPT
        public async Task<IActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RequestsVM.Requests = await _db.Requests.Include(m => m.ProblemTypes).SingleOrDefaultAsync(m => m.Id == id);

            if (RequestsVM.Requests == null)
            {
                return NotFound();
            }

            return View(RequestsVM);
            //need to be put in a list or somehting, and possbile have stages of progress. Also comment from mech?
        }

        // POST ACCEPT Action Method   --------------------------------    ACCEPT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            if (ModelState.IsValid)
            {
                var requestFromDb = _db.Requests.Where(m => m.Id == RequestsVM.Requests.Id).FirstOrDefault();
                var customerUser = _db.ApplicationUser.Where(u => u.Id == requestFromDb.CustomerId).Single();

                var mechanic = _db.Mechanics.Where(m => m.Id == requestFromDb.MechanicAssigned).Single();
                var mechanicUser = _db.ApplicationUser.Where(u => u.Id == mechanic.UserId).Single();
                //var customerUser = await _userManager.FindByIdAsync(requestFromDb.CustomerId);
                //var customerEmail = await _userManager.GetEmailAsync(customerUser);
                //if no user, try catch some error

                requestFromDb.AcceptedByMechanic = RequestsVM.Requests.AcceptedByMechanic;

                await _db.SaveChangesAsync();

                // SEND Email about accepted request to Customer.
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(mechanic.Name, mechanicUser.Email);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(requestFromDb.FirstName + " " + requestFromDb.LastName, customerUser.Email);
                message.To.Add(to);

                message.Subject = "Request scheduled on date " + requestFromDb.RequestScheduledDate + " was accepted";

                //BodyBuilder bodyBuilder = new BodyBuilder();
                //bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
                //bodyBuilder.TextBody = "Hello World!";
                //message.Body = bodyBuilder.ToMessageBody();
                message.Body = new TextPart("plain")
                {
                    Text = @"Hello, " + customerUser.LastName + " Your " + requestFromDb.ProblemTypes.Name + " request on the date " + requestFromDb.RequestScheduledDate + " was accepted by " + mechanic.Name + ".\r" + "Here's the messaged that was sent to " + mechanic.Name + ":\r" + requestFromDb.Message + ".\r\r" + "Have a pleasant day."
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("rsha.noreply@gmail.com", _configuration["RSHAEmail:EmailPassword"]);

                    client.Send(message);
                    client.Disconnect(true);

                    return RedirectToAction(nameof(Index));
                }
            }
            
            return View(RequestsVM);
        }

        // DELETE Action Method       --------------------------------    DELETE
        public async Task<IActionResult> Delete()
        {
            return View();

            //just delete request and send notification and email to customer notifying them of cannon accept this request
        }

        // FINISH Action Method       --------------------------------    FINISH
        public async Task<IActionResult> Finish()
        {
            return View();
        }

    }
}