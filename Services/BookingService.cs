using Supabase;
using Supabase.Postgrest;
using System;
using System.Linq;
using System.Threading.Tasks;
using SpeedSolution.Models;
using System.Collections.Generic;

namespace SpeedSolution.Services
{
    public class BookingService
    {
        private readonly SupabaseClientService _supabase;
        public BookingService(SupabaseClientService supabase) => _supabase = supabase;

        public async Task<List<Booking>> GetAllAsync()
        {
            var res = await _supabase.Client.From<Booking>().Get();
            return res.Models;
        }

        public async Task<Booking> GetByIdAsync(Guid id)
        {
            var res = await _supabase.Client.From<Booking>().Where(b => b.Id == id).Get();
            return res.Models.FirstOrDefault();
        }

        public async Task<bool> CreateAsync(Booking booking)
        {
            var res = await _supabase.Client.From<Booking>().Insert(booking);
            return res.Models.Count > 0;
        }
    }
}
