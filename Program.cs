using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Supabase;
using SpeedSolution.Services;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session service
builder.Services.AddSession(options =>
{
    options.IdleTimeout = System.TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register Supabase client service and app services
builder.Services.AddSingleton<SupabaseClientService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EngineerService>();
builder.Services.AddScoped<BookingService>();

var app = builder.Build();

// Initialize Supabase client at startup
var supabaseSvc = app.Services.GetRequiredService<SupabaseClientService>();
await supabaseSvc.InitializeAsync();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Use Session before Authorization
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
