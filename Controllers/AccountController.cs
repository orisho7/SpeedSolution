using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpeedSolution.Services;
using SpeedSolution.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http; // Added for explicit Session extension resolution if needed, though usually included

namespace SpeedSolution.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _users;
        private readonly BookingService _bookings;

        public AccountController(UserService users, BookingService bookings)
        {
            _users = users;
            _bookings = bookings;
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

                // Debug: Show all mobile numbers in database
                Console.WriteLine($"=== LOGIN DEBUG ===");
                Console.WriteLine($"Searching for mobile: '{mobile}' (length: {mobile?.Length})");
                Console.WriteLine($"Total users in DB: {allUsers.Count}");
                foreach (var u in allUsers)
                {
                    Console.WriteLine($"  - User: {u.FirstName} {u.SecondName}, Mobile: '{u.Mobile}' (length: {u.Mobile?.Length}), Email: {u.Email}");
                }

                var user = await _users.ValidateCredentialsByMobileAsync(mobile, password);
                if (user == null)
                {
                    // Show first 3 mobile numbers for debugging
                    var sampleMobiles = string.Join(", ", allUsers.Take(3).Select(u => $"'{u.Mobile}'"));
                    ViewBag.Error = $"Invalid mobile number or password. Database has {allUsers.Count} users. Sample mobiles: {sampleMobiles}";
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
                Console.WriteLine($"=== REGISTRATION DEBUG ===");
                Console.WriteLine($"Attempting to register user: {model.FirstName} {model.SecondName}");
                Console.WriteLine($"Mobile: '{model.Mobile}', Email: '{model.Email}'");
                Console.WriteLine($"Password: '{model.Password}'");
                
                // DON'T set Id and Created_At - let the database auto-generate them
                // model.Id = Guid.NewGuid();  // REMOVED - database will auto-generate
                // model.Created_At = DateTime.UtcNow;  // REMOVED - database has default now()
                
                Console.WriteLine($"Letting database auto-generate ID and timestamp");
                
                // Check users before insert
                var beforeCount = (await _users.GetAllAsync())?.Count ?? 0;
                Console.WriteLine($"Users in DB before insert: {beforeCount}");
                
                // NOTE: hash password in production
                var ok = await _users.CreateAsync(model);
                
                Console.WriteLine($"CreateAsync returned: {ok}");
                
                if (!ok)
                {
                    ViewBag.Error = "Registration failed. Please try again.";
                    Console.WriteLine("Registration failed - CreateAsync returned false");
                    return View();
                }

                // Check users after insert
                var afterCount = (await _users.GetAllAsync())?.Count ?? 0;
                Console.WriteLine($"Users in DB after insert: {afterCount}");
                
                // Verify by checking if count increased
                if (afterCount > beforeCount)
                {
                    Console.WriteLine($"✅ SUCCESS! User count increased from {beforeCount} to {afterCount}");
                    Console.WriteLine($"User registered: {model.FirstName} {model.SecondName}, Mobile: '{model.Mobile}'");
                }
                else
                {
                    Console.WriteLine($"⚠️ WARNING: User count did not increase! Before: {beforeCount}, After: {afterCount}");
                }

                TempData["SuccessMessage"] = "Registration successful! Please login with your credentials.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Registration error: {ex.Message}";
                Console.WriteLine($"=== REGISTRATION EXCEPTION ===");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return View();
            }
        }


        // GET: /Account/Profile
        public async Task<IActionResult> Profile()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login");
            }

            var userId = Guid.Parse(userIdString);
            var user = await _users.GetByIdAsync(userId);
            
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Fetch user's bookings
            var allBookings = await _bookings.GetAllAsync();
            var userBookings = allBookings?.Where(b => b.User_Id == userId).OrderByDescending(b => b.Created_At).ToList();
            
            ViewBag.Bookings = userBookings ?? new List<Booking>();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login");
            }

            try
            {
                var userId = Guid.Parse(userIdString);
                
                // Update only allowed fields
                var existingUser = await _users.GetByIdAsync(userId);
                if (existingUser == null)
                {
                    return RedirectToAction("Login");
                }

                existingUser.FirstName = model.FirstName;
                existingUser.SecondName = model.SecondName;
                existingUser.Mobile = model.Mobile;
                existingUser.Email = model.Email;
                existingUser.City = model.City;

                var success = await _users.UpdateAsync(userId, existingUser);
                
                if (success)
                {
                    // Update session with new name
                    HttpContext.Session.SetString("UserName", $"{existingUser.FirstName} {existingUser.SecondName}");
                    ViewBag.Message = "Profile updated successfully!";
                }
                else
                {
                    ViewBag.Error = "Failed to update profile.";
                }

                return View(existingUser);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error updating profile: {ex.Message}";
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login");
            }

            try
            {
                var userId = Guid.Parse(userIdString);
                
                // Get the booking to verify it belongs to the user
                var booking = await _bookings.GetByIdAsync(id);
                if (booking == null || booking.User_Id != userId)
                {
                    TempData["ErrorMessage"] = "Booking not found or you don't have permission to delete it.";
                    return RedirectToAction("Profile");
                }

                // Delete the booking
                await _bookings.DeleteAsync(id);
                
                TempData["SuccessMessage"] = "Booking deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting booking: {ex.Message}";
            }

            return RedirectToAction("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
