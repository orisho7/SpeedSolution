using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpeedSolution.Services;

namespace SpeedSolution.Controllers
{
    public class EngineersController : Controller
    {
        private readonly EngineerService _engineers;
        private readonly BookingService _bookings;
        
        public EngineersController(EngineerService engineers, BookingService bookings)
        {
            _engineers = engineers;
            _bookings = bookings;
        }

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
                User_Id = userId,
                Engineer_Id = EngineerId,
                Date = Date,
                Time = Time,
                EngineerName = EngineerName,
                Payment_Method = PaymentMethod,
                Payment_Status = "Accepted"
            };

            // Save to database
            try
            {
                var success = await _bookings.CreateAsync(booking);
                
                if (success)
                {
                    TempData["SuccessMessage"] = $"Booking confirmed with {EngineerName} on {Date:yyyy-MM-dd} at {Time}!";
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    ViewBag.Error = "Failed to create booking. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error creating booking: {ex.Message}";
            }

            var engineer = await _engineers.GetByIdAsync(EngineerId);
            return View(engineer);
        }
    }
}
