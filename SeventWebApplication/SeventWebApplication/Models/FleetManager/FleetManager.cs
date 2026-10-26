
using SeventWebApplication.Models.Booking;
using SeventWebApplication.Models.VehicleNS;
using System;

public class FleetManager
{
    private static FleetManager? _instance = null;

    public static FleetManager instance
    {
        // Checks if the instance is not null and returns it or creates a new one if it is null
        get { return _instance ??= new FleetManager(); }
    }

    private Dictionary<string, Vehicle> _vehicles = new();// <reg-num, vehicle>
    private List<Booking> _bookings = new();

    // Private constructor so only one instance can be created (using the instance getter)
    private FleetManager() { }

    // Vet inte om den borde ta in en Vehicle eller delar av en Vehicle, som reg-num, typ, etc
#if false

    public void AddVehicle(Vehicle vehicle)
    {
        if(vehicle.RegNr.Count() != 6 || vehicle.RegNr.Count() != 7) 
        {
            // Ge en varning att det inte är ett svenskt regnummer? borde nog vara högre upp
        }
        _vehicles.Add(vehicle.RegNr, vehicle);
    }

#endif

    public IEnumerable<Vehicle> GetAvailableVehicles(DateTime startTime, DateTime endTime)
    {
        var availableVehicles = new List<Vehicle>();

        foreach (var vehicle in _vehicles.Values)
        {
            if (IsAvailable(vehicle, startTime, endTime))
            {
                availableVehicles.Add(vehicle);
            }
        }

        return availableVehicles;
    }

    // NYTT: Söker fordon utifrån bilmärke eller modell.
    // Används av HomeController när söktext skickas in som query.
    public IEnumerable<Vehicle> FindVehicles(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<Vehicle>();

        return _vehicles.Values.Where(vehicle =>
            vehicle.Model.Brand.BrandName.Contains(
                query, StringComparison.OrdinalIgnoreCase)
            ||
            vehicle.Model.ModelName.Contains(
                query, StringComparison.OrdinalIgnoreCase));
    }

    public bool IsAvailable(Vehicle vehicle, DateTime startDate, DateTime endDate)
    {
        foreach (var booking in _bookings)
        {
            //Regplåtsobjekt
#if false
            if( booking.Vehicle.RegNr != vehicle.RegNr )
                continue;
#endif

            // Checks if startDate is inside the booking
            if (startDate.CompareTo(booking.StartDate) > 0 && startDate.CompareTo(booking.EndDate) < 0)
                return false;
            // Checks if endDate is inside the booking
            if (endDate.CompareTo(booking.StartDate) > 0 && endDate.CompareTo(booking.EndDate) < 0)
                return false;
            // If startDate is before the booking.StartDate AND endDate is after the booking.EndDate
            if (startDate.CompareTo(booking) < 0 && endDate.CompareTo(booking.EndDate) > 0)
                return false;

        }
        return true;
    }

    public void RegisterBooking(Booking booking)// vill nog ha några andra parametrar?
    {
        // TODO: Implementera när booking är klar

        _bookings.Add(booking); //Temp
    }

    public IEnumerable<Booking> GetBookings()
    {
        return _bookings;
    }


    // TESTDATA: Skapar en dummybil för att testa sökfunktionen.
    public void SeedTestVehicle()
    {
        var brand = new VehicleBrand("Volvo");

        // ÄNDRING: VehicleModel tar emot ett Guid (BrandId),
        // inte ett VehicleBrand-objekt.
        var model = new VehicleModel("V60", brand.BrandId)
        {
            Brand = brand
        };

        var registrationPlate = new RegistrationPlate(
            "ABC123",
            CountryCode.SE
        );

        var vehicle = new Car(
            registrationPlate,
            model,

            // ÄNDRING: Ersätt med ett värde som faktiskt finns
            // i din VehicleCategoryType-enum.
            VehicleCategoryType.Car,

            2024,
            197,
            5,
            529,
            FuelType.Petrol,
            TransmissionType.Automatic,
            PassengerCarCategoryType.SUV,
            VehicleComfortType.Standard
        );

        _vehicles[registrationPlate.RegNr] = vehicle;
    }

}
