using Supabase.Postgrest;
using Supabase.Postgrest.Models;
using System;
using System.Threading.Tasks;
using SpeedSolution.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpeedSolution.Services
{
    public class UserService
    {
        private readonly SupabaseClientService _supabase;

        public UserService(SupabaseClientService supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<User>> GetAllAsync()
        {
            Console.WriteLine($"[UserService.GetAllAsync] Fetching all users...");
            
            try
            {
                var query = _supabase.Client.From<User>();
                var res = await query.Get();
                
                Console.WriteLine($"[UserService.GetAllAsync] Retrieved {res.Models?.Count ?? 0} users");
                
                if (res.Models != null && res.Models.Count > 0)
                {
                    Console.WriteLine($"[UserService.GetAllAsync] First user: {res.Models[0].FirstName} {res.Models[0].SecondName}");
                }
                
                return res.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService.GetAllAsync] ERROR: {ex.Message}");
                Console.WriteLine($"[UserService.GetAllAsync] ERROR Type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[UserService.GetAllAsync] Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            Console.WriteLine($"[UserService.GetByIdAsync] Fetching user with ID: {id}");
            try
            {
                var res = await _supabase.Client.From<User>().Where(u => u.Id == id).Get();
                Console.WriteLine($"[UserService.GetByIdAsync] Found {res.Models?.Count ?? 0} users");
                return res.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService.GetByIdAsync] ERROR: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CreateAsync(User user)
        {
            Console.WriteLine($"[UserService.CreateAsync] Inserting user: {user.FirstName} {user.SecondName}, Mobile: '{user.Mobile}'");
            try
            {
                var res = await _supabase.Client.From<User>().Insert(user);
                Console.WriteLine($"[UserService.CreateAsync] Insert returned {res.Models?.Count ?? 0} models");
                var success = res.Models.Count > 0;
                Console.WriteLine($"[UserService.CreateAsync] Success: {success}");
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService.CreateAsync] ERROR: {ex.Message}");
                Console.WriteLine($"[UserService.CreateAsync] StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[UserService.CreateAsync] Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, User user)
        {
            user.Id = id;
            var res = await _supabase.Client.From<User>().Update(user);
            return res.Models.Count > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _supabase.Client.From<User>().Where(u => u.Id == id).Delete();
            return true;
        }

        public async Task<User> ValidateCredentialsAsync(string email, string password)
        {
            var res = await _supabase.Client.From<User>().Where(u => u.Email == email).Get();
            var user = res.Models.FirstOrDefault();
            if (user == null) return null;

            if (user.Password == password) return user;
            return null;
        }

        public async Task<User> ValidateCredentialsByMobileAsync(string mobile, string password)
        {
            Console.WriteLine($"[UserService] Validating mobile: '{mobile}', password: '{password}'");
            
            var res = await _supabase.Client.From<User>().Where(u => u.Mobile == mobile).Get();
            
            Console.WriteLine($"[UserService] Query returned {res.Models?.Count ?? 0} results");
            
            var user = res.Models.FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine($"[UserService] No user found with mobile: '{mobile}'");
                return null;
            }

            Console.WriteLine($"[UserService] Found user: {user.FirstName} {user.SecondName}, Mobile: '{user.Mobile}', Password match: {user.Password == password}");
            
            if (user.Password == password) return user;
            
            Console.WriteLine($"[UserService] Password mismatch. Expected: '{user.Password}', Got: '{password}'");
            return null;
        }
    }
}
