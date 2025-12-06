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
            var res = await _supabase.Client.From<User>().Get();
            return res.Models;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var res = await _supabase.Client.From<User>().Where(u => u.Id == id).Get();
            return res.Models.FirstOrDefault();
        }

        public async Task<bool> CreateAsync(User user)
        {
            var res = await _supabase.Client.From<User>().Insert(user);
            return res.Models.Count > 0;
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
            var res = await _supabase.Client.From<User>().Where(u => u.Mobile == mobile).Get();
            var user = res.Models.FirstOrDefault();
            if (user == null) return null;

            if (user.Password == password) return user;
            return null;
        }
    }
}
