using Supabase;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace SpeedSolution.Services
{
    public class SupabaseClientService
    {
        public Client Client { get; private set; }

        public SupabaseClientService(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"];
            var key = configuration["Supabase:Key"];
            
            // Fallback to environment variables if config is missing or has placeholders
            if (string.IsNullOrEmpty(url) || url.Contains("YOUR_SUPABASE_URL"))
            {
                url = System.Environment.GetEnvironmentVariable("SUPABASE_URL");
            }

            if (string.IsNullOrEmpty(key) || key.Contains("YOUR_SUPABASE_ANON_KEY"))
            {
                key = System.Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY");
            }

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
            {
                // Optionally throw or log, but for now allow it to proceed (initialization might fail later)
            }

            var options = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            Client = new Client(url, key, options);
        }

        public async Task InitializeAsync()
        {
            await Client.InitializeAsync();
        }
    }
}
