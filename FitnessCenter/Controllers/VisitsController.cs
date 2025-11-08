using FitnessCenter.Data;
using FitnessCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Controllers
{
    public class VisitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visits
        [ResponseCache(Duration = 284, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index()
        {
            var visits = _context.Visits
                .Include(v => v.Client)
                .OrderByDescending(v => v.CheckInTime)
                .ToList();

            return View(visits);
        }

        // GET: Visits/Create — не кэшируем, чтобы форма всегда была свежей
        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients
                .OrderBy(c => c.LastName)
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.ClientID.ToString(),
                    Text = $"{c.LastName} {c.FirstName}"
                }).ToList();
            return View();
        }

        // POST: Visits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ClientID,CheckInTime,CheckOutTime")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visit);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(visit);
        }
    }
}
