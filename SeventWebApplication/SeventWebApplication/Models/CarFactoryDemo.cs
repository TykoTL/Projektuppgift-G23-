
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
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            FuelType fuelType,
            TransmissionType transmissionType,
            CountryCode countryCode,
            PassengerCarCategoryType passengerCarCategoryType,
            VehicleComfortType vehicleComfortType)
        {
            var car = (Car)_carFactory.CreateStandard(
                regNr,
                model,
                vehicleCategoryType,
                year,
                horsePower,
                passengerCapacity,
                luggageCapacity,
                fuelType,
                transmissionType,
                countryCode,
                passengerCarCategoryType,
                vehicleComfortType);

            _createdVehicles.Add(car);
            return car;
        }

        public ElectricCar CreateElectricCar(
            string regNr,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            FuelType fuelType,
            TransmissionType transmissionType,
            CountryCode countryCode,
            PassengerCarCategoryType passengerCarCategoryType,
            VehicleComfortType vehicleComfortType,
            int batteryCapacityKwh,
            int rangeKm)
        {
            var baseCar = _carFactory.CreateStandard(
                regNr,
                model,
                vehicleCategoryType,
                year,
                horsePower,
                passengerCapacity,
                luggageCapacity,
                fuelType,
                transmissionType,
                countryCode,
                passengerCarCategoryType,
                vehicleComfortType);

            var electricCar = (ElectricCar)_carFactory.CreateElectric(
                baseCar,
                batteryCapacityKwh,
                rangeKm);

            _createdVehicles.Add(electricCar);
            return electricCar;
        }

        public IReadOnlyList<Vehicle> GetAllCreated()
            => _createdVehicles.AsReadOnly();
    }
}
