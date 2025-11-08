using FitnessCenter.Data;
using FitnessCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        [ResponseCache(Duration = 284, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                .Include(c => c.Sales!)
                .ThenInclude(s => s.MembershipPlan)
                .ToListAsync();

            return View(clients);
        }

        // GET: Clients/Details/5
        [ResponseCache(Duration = 284, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _context.Clients
                .Include(c => c.Sales!)
                .ThenInclude(s => s.MembershipPlan)
                .Include(c => c.Visits)
                .FirstOrDefaultAsync(c => c.ClientID == id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // GET: Clients/Create (не кэшируем формы!)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,MiddleName,BirthDate,Gender,Phone,Email")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
    }
}
