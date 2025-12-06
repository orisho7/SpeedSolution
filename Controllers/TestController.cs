using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpeedSolution.Services;
using System;

namespace SpeedSolution.Controllers
{
    public class TestController : Controller
    {
        private readonly UserService _users;
        private readonly EngineerService _engineers;

        public TestController(UserService users, EngineerService engineers)
        {
            _users = users;
            _engineers = engineers;
        }

        // GET: /Test/Database
        public async Task<IActionResult> Database()
        {
            try
            {
                var users = await _users.GetAllAsync();
                var engineers = await _engineers.GetAllAsync();

                var result = new
                {
                    Success = true,
                    UsersCount = users?.Count ?? 0,
                    EngineersCount = engineers?.Count ?? 0,
                    Message = $"Database connected! Found {users?.Count ?? 0} users and {engineers?.Count ?? 0} engineers.",
                    SampleUsers = users?.Take(3).Select(u => new { u.FirstName, u.Mobile, u.Email }).ToList(),
                    SampleEngineers = engineers?.Take(3).Select(e => new { e.FirstName, e.LastName, e.Category, e.Price }).ToList()
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                var error = new
                {
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace?.Substring(0, Math.Min(500, ex.StackTrace?.Length ?? 0)),
                    InnerException = ex.InnerException?.Message
                };

                return Json(error);
            }
        }
    }
}
