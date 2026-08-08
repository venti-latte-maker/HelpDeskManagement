using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // HelpDesk.Api's "http" launch profile runs on http://localhost:5214 (see
            // HelpDesk.Api/Properties/launchSettings.json) - this previously pointed at
            // https://localhost:5271, which is this MVC app's own port, not the API's.
            builder.Services.AddHttpClient<TicketService>(
                c => c.BaseAddress = new Uri("http://localhost:5214/api/Ticket/"));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
