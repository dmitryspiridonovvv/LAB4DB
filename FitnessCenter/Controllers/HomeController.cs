using FitnessCenter.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Главная страница кэшируется на 284 секунды
        [ResponseCache(Duration = 284, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                .Include(c => c.Sales)
                .ThenInclude(s => s.MembershipPlan)
                .ToListAsync();

            return View(clients);
        }

        [ResponseCache(Duration = 284, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
