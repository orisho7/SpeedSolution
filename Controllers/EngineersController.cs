using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpeedSolution.Services;

namespace SpeedSolution.Controllers
{
    public class EngineersController : Controller
    {
        private readonly EngineerService _engineers;
        public EngineersController(EngineerService engineers) => _engineers = engineers;

        public async Task<IActionResult> Index(string category, int? page)
        {
            var allEngineers = await _engineers.GetAllAsync();

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
            {
                allEngineers = allEngineers.Where(e => e.Category == category).ToList();
            }

            // Pagination
            int pageSize = 6;
            int currentPage = page ?? 1;
            int totalPages = (int)Math.Ceiling(allEngineers.Count / (double)pageSize);

            var pagedEngineers = allEngineers
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get unique categories for sidebar
            var allCategories = (await _engineers.GetAllAsync())
                .Select(e => e.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            ViewBag.Categories = allCategories;
            ViewBag.Category = category;
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(pagedEngineers);
        }


        public async Task<IActionResult> Details(System.Guid id)
        {
            var item = await _engineers.GetByIdAsync(id);
            return View(item);
        }

        // GET: /Engineers/Book/{id}
        public async Task<IActionResult> Book(System.Guid id)
        {
            var engineer = await _engineers.GetByIdAsync(id);
            if (engineer == null)
            {
                return NotFound();
            }
            return View(engineer);
        }

        // POST: /Engineers/Book
        [HttpPost]
        public async Task<IActionResult> Book(System.Guid EngineerId, string EngineerName, DateTime Date, string Time, int Hours, decimal TotalPrice, string PaymentMethod)
        {
            // Get current user ID from session or claims
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = Guid.Parse(userIdString);

            // Create booking
            var booking = new Models.Booking
            {
                Id = Guid.NewGuid(),
                User_Id = userId,
                Engineer_Id = EngineerId,
                Date = Date,
                Time = Time,
                EngineerName = EngineerName,
                Payment_Method = PaymentMethod,
                Payment_Status = "Pending",
                Created_At = DateTime.UtcNow
            };

            // Save to database (you'll need to inject BookingService)
            // For now, we'll redirect with a success message

            var engineer = await _engineers.GetByIdAsync(EngineerId);
            ViewBag.ConfirmationMessage = $"Booking confirmed with {EngineerName} on {Date:yyyy-MM-dd} at {Time}!";

            return View(engineer);
        }
    }
}
