using Microsoft.AspNetCore.Mvc;

namespace SpeedSolution.Controllers
{
    public class ContactUsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(string note)
        {
            // Mock submission
            TempData["Message"] = "Your message has been sent successfully!";
            return RedirectToAction("Index");
        }
    }
}
