using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpeedSolution.Services;
using SpeedSolution.Models;
using System;
using Microsoft.AspNetCore.Http; // Added for explicit Session extension resolution if needed, though usually included

namespace SpeedSolution.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _users;

        public AccountController(UserService users)
        {
            _users = users;
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string mobile, string password)
        {
            try
            {
                // Debug: Check if any users exist
                var allUsers = await _users.GetAllAsync();
                if (allUsers == null || allUsers.Count == 0)
                {
                    ViewBag.Error = "No users found in database. Please register first or run the database seed script.";
                    return View();
                }

                // Debug: Log attempt
                Console.WriteLine($"Login attempt - Mobile: {mobile}, Users in DB: {allUsers.Count}");

                var user = await _users.ValidateCredentialsByMobileAsync(mobile, password);
                if (user == null)
                {
                    ViewBag.Error = $"Invalid mobile number or password. ({allUsers.Count} users in database)";
                    return View();
                }

                // Simple session example (for demo only)
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserName", user.FirstName + " " + user.SecondName);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Login error: {ex.Message}";
                Console.WriteLine($"Login exception: {ex}");
                return View();
            }
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                model.Created_At = DateTime.UtcNow;
                // NOTE: hash password in production
                var ok = await _users.CreateAsync(model);
                if (!ok)
                {
                    ViewBag.Error = "Registration failed. Please try again.";
                    return View();
                }

                TempData["SuccessMessage"] = "Registration successful! Please login with your credentials.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Registration error: {ex.Message}";
                Console.WriteLine($"Registration exception: {ex}");
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
