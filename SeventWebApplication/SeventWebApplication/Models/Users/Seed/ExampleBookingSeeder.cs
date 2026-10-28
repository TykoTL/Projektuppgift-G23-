using SeventWebApplication.Models.Addon;
using SeventWebApplication.Models.VehicleNS;
using SeventWebApplication.Models.VehicleNS.Engine;
using BookingModel = SeventWebApplication.Models.Booking.Booking;

namespace SeventWebApplication.Models.Users
{
    // vibe code seeder som skapar några exempelbokningar för att kunna testa bokningsvisaren.
    public static class ExampleBookingSeeder
    {
        public static void Seed(IUserRepository users)
        {
            var fleet = global::FleetManager.instance;

            if (fleet.GetBookings().Any())
                return;

            var allUsers = users.GetAll().ToList();
            if (allUsers.Count == 0)
                return;

            var random = new Random();
            var cars = new[]
            {
                MakeCar("Volvo", "V60", "ABC123", VehicleNS.PassengerCarCategoryType.SUV, 2024, 197, FuelType.Petrol, 529m),
                MakeCar("Tesla", "Model 3", "DEF456", VehicleNS.PassengerCarCategoryType.Sedan, 2023, 283, FuelType.Electric, 425m),
                MakeCar("Volkswagen", "Golf", "GLF777", VehicleNS.PassengerCarCategoryType.Hatchback, 2022, 150, FuelType.Diesel, 380m),
                MakeCar("BMW", "i3", "BMW999", VehicleNS.PassengerCarCategoryType.Sedan, 2022, 250, FuelType.Electric, 300m)
            };

            var statuses = new[] { "Pending", "Confirmed", "Active", "Completed", "Cancelled" };

            for (int i = 0; i < 10; i++)
            {
                var user = allUsers[random.Next(allUsers.Count)];
                var car = cars[random.Next(cars.Length)];
                var status = statuses[random.Next(statuses.Length)];

                var start = DateTime.Today.AddDays(random.Next(-20, 25));
                var end = start.AddDays(random.Next(1, 7));

                fleet.RegisterBooking(new BookingModel(
                    Guid.NewGuid().ToString(),
                    user.UserId.ToString(),
                    car,
                    start,
                    end,
                    RandomAddons(random, new VehicleBase(500m + i * 75m)))
                {
                    Status = status
                });
            }
        }

        // Slumpar fram tillval och lindar dem runt grundpriset (Decorator).
        // T.ex. kan det bli ChildSeatAddon(GPSAddon(VehicleBase)), och GetPrice() räknar ihop allt.
        // Max 3 barnstolar, samma gräns som i AddonValidationStrategy.
        private static IAddon RandomAddons(Random random, IAddon price)
        {
            if (random.Next(2) == 0)                 // GPS i ungefär hälften av bokningarna
                price = new GPSAddon(price);

            int childSeats = random.Next(0, 4);      // 0–3 barnstolar
            for (int i = 0; i < childSeats; i++)
                price = new ChildSeatAddon(price);

            if (random.Next(3) == 0)                 // extra förare i ungefär var tredje
                price = new ExtraDriverAddon(price);

            return price;
        }

        private static Car MakeCar(
            string brandName,
            string modelName,
            string regNr,
            VehicleNS.PassengerCarCategoryType passengerCarCategoryType,
            int year,
            int horsePower,
            FuelType fuel,
            decimal luggage)
        {
            var brand = new VehicleBrand(brandName);
            var model = new VehicleModel(modelName, brand.BrandId) { Brand = brand };

            IEngine engine = fuel switch
            {
                FuelType.Petrol => new PetrolEngine(horsePower, 50),
                FuelType.Diesel => new DieselEngine(horsePower, 50),
                FuelType.Electric => new ElectricEngine(horsePower, 60, 350),
                _ => throw new ArgumentOutOfRangeException(nameof(fuel), fuel, "Unsupported fuel type.")
            };

            return new Car(
                new RegistrationPlate(regNr, CountryCode.SE),
                model,
                VehicleCategoryType.PassengerCar,
                passengerCarCategoryType,
                VehicleNS.VehicleComfortType.Standard,
                year,
                5,
                luggage,
                engine,
                TransmissionType.Automatic);
        }
    }
}
