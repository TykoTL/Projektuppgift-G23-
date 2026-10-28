using SeventWebApplication.Models.VehicleNS;

namespace SeventWebApplication.Demo
{
    public class CarFactoryDemo
    {
        private readonly CarFactory _carFactory;
        private readonly List<Vehicle> _createdVehicles = new();

        public CarFactoryDemo()
        {
            var countryValidator = RegNrValidatorFactory.Create(CountryCode.SE);

            _carFactory = new CarFactory(
                countryValidator,
                new BrandModelValidator(),
                new YearValidator(),
                new HorsePowerValidator()
            );
        }

        public Car CreateCar(
            string regNr,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            Models.VehicleNS.VehicleComfortType vehicleComfortType,
            Models.VehicleNS.PassengerCarCategoryType passengerCarCategoryType,
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            FuelType fuelType,
            TransmissionType transmissionType,
            CountryCode countryCode)
        {
            var car = (Car)_carFactory.CreateStandard(
                regNr,
                model,
                vehicleCategoryType,
                passengerCarCategoryType,
                vehicleComfortType, 
                year,
                horsePower,
                passengerCapacity,
                luggageCapacity,
                fuelType,
                transmissionType,
                countryCode);

            _createdVehicles.Add(car);
            return car;
        }


        public IReadOnlyList<Vehicle> GetAllCreated()
            => _createdVehicles.AsReadOnly();
    }
}