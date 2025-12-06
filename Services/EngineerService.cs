using Supabase;
using Supabase.Postgrest;
using System;
using System.Linq;
using System.Threading.Tasks;
using SpeedSolution.Models;
using System.Collections.Generic;

namespace SpeedSolution.Services
{
    public class EngineerService
    {
        private readonly SupabaseClientService _supabase;
        public EngineerService(SupabaseClientService supabase) => _supabase = supabase;

        public async Task<List<Engineer>> GetAllAsync()
        {
            var res = await _supabase.Client.From<Engineer>().Get();
            return res.Models;
        }

        public async Task<Engineer> GetByIdAsync(Guid id)
        {
            var res = await _supabase.Client.From<Engineer>().Where(e => e.Id == id).Get();
            return res.Models.FirstOrDefault();
        }

        public async Task<bool> CreateAsync(Engineer engineer)
        {
            var res = await _supabase.Client.From<Engineer>().Insert(engineer);
            return res.Models.Count > 0;
        }
    }
}
