using SeventWebApplication.Models.Booking;
using SeventWebApplication.Models.Users;
using SeventWebApplication.Models.VehicleNS;

namespace SeventWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ---- Användardelen ----
            // Singleton = samma objekt i hela appen, så alla delar samma användarlista.
            builder.Services.AddSingleton<IUserRepository>(_ =>
            {
                var repo = new UserRepository();
                repo.Seed(); // kladd metod som seedar några exempel account
                return repo;
            });
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<AccountService>();
            builder.Services.AddSingleton<BookingViewerService>();

            // Session = så att inloggningen hänger kvar mellan sidladdningar (via en cookie boom)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;                 // JavaScript kommer inte åt cookien
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();
            // Scoped = nytt UserSession-objekt för varje sidladdning
            builder.Services.AddScoped<UserSession>();

            var app = builder.Build();

            // testmetod som skapar några exempelbokningar fördelade på kunderna i repot,
            // så man har nåt att kolla på under /Bookings
            ExampleBookingSeeder.Seed(app.Services.GetRequiredService<IUserRepository>());

            // ---- Bokningsdelen ----
            builder.Services.AddSingleton<BookingRepository>();

            // ---- Fordonsdelen ----
            builder.Services.AddSingleton<VehicleRentalRecordRepository>();
            builder.Services.AddSingleton<VehicleInspectionRepository>();
            builder.Services.AddSingleton<VehicleDamageRepository>();
            builder.Services.AddSingleton<FleetManager>();
            builder.Services.AddSingleton<PricingService>();
            builder.Services.AddScoped<VehicleDamageService>();
            builder.Services.AddScoped<RentalReturnService>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseSession(); // måste komma efter UseRouting, annars funkar inte sessionen,
                              // behöver köra i den delen av pipelinen där requesten redan är “routad” och där appen vet vilken endpoint som kommer att köras.

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
