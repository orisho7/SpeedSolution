using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpeedSolution.Services;
using SpeedSolution.Models;
using System;

namespace SpeedSolution.Controllers
{
    public class BookingsController : Controller
    {
        private readonly BookingService _bookings;
        public BookingsController(BookingService bookings) => _bookings = bookings;

        public async Task<IActionResult> Index()
        {
            var list = await _bookings.GetAllAsync();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Booking model)
        {
            model.Created_At = DateTime.UtcNow;
            model.Id = Guid.NewGuid();
            var ok = await _bookings.CreateAsync(model);
            if (!ok)
            {
                ViewBag.Error = "Booking failed";
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
