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
            Console.WriteLine("=== SUPABASE CLIENT INITIALIZATION ===");
            
            var url = configuration["Supabase:Url"];
            var key = configuration["Supabase:Key"];
            
            Console.WriteLine($"Config URL: {url}");
            Console.WriteLine($"Config Key: {(string.IsNullOrEmpty(key) ? "MISSING" : $"{key.Substring(0, Math.Min(20, key.Length))}...")}");
            
            // Fallback to environment variables if config is missing or has placeholders
            if (string.IsNullOrEmpty(url) || url.Contains("YOUR_SUPABASE_URL"))
            {
                url = System.Environment.GetEnvironmentVariable("SUPABASE_URL");
                Console.WriteLine($"Using environment variable for URL: {url}");
            }

            if (string.IsNullOrEmpty(key) || key.Contains("YOUR_SUPABASE_ANON_KEY"))
            {
                key = System.Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY");
                Console.WriteLine($"Using environment variable for Key");
            }

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
            {
                Console.WriteLine("ERROR: Supabase URL or Key is missing!");
                throw new InvalidOperationException("Supabase URL and Key must be configured in appsettings.json or environment variables");
            }

            var options = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            Client = new Client(url, key, options);
            Console.WriteLine("Supabase Client created successfully");
        }

        public async Task InitializeAsync()
        {
            Console.WriteLine("Initializing Supabase Client...");
            await Client.InitializeAsync();
            Console.WriteLine("Supabase Client initialized successfully!");
        }
    }
}
