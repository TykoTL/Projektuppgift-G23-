using SeventWebApplication.Models.VehicleNS.Engine;

namespace SeventWebApplication.Models.VehicleNS
{
    public class CarFactory : IVehicleFactory
    {
        private readonly IRegNrValidator _regNrValidator;
        private readonly IBrandModelValidator _brandModelValidator;
        private readonly IYearValidator _yearValidator;
        private readonly IHorsePowerValidator _horsePowerValidator;

        public CarFactory(
            IRegNrValidator regNrValidator,
            IBrandModelValidator brandModelValidator,
            IYearValidator yearValidator,
            IHorsePowerValidator horsePowerValidator)
        {
            _regNrValidator = regNrValidator;
            _brandModelValidator = brandModelValidator;
            _yearValidator = yearValidator;
            _horsePowerValidator = horsePowerValidator;
        }

        public Vehicle CreateStandard(
            string regNr,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            PassengerCarCategoryType passengerCarCategoryType,
            VehicleComfortType vehicleComfortType,
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            FuelType fuelType,
            TransmissionType transmissionType,
            CountryCode countryCode)
        {
            ValidateCommonFields(regNr, model, year, horsePower);

            var registrationPlate = new RegistrationPlate(regNr, countryCode);

            IEngine engine = fuelType switch
            {
                FuelType.Petrol => new PetrolEngine(horsePower, 50),
                FuelType.Diesel => new DieselEngine(horsePower, 50),
                _ => throw new ArgumentException(
                    "Använd CreateElectric eller en annan lämplig metod för denna motortyp.",
                    nameof(fuelType))
            };

            return new Car(
                registrationPlate,
                model,
                vehicleCategoryType,
                passengerCarCategoryType,
                vehicleComfortType, // Tillagt
                year,
                passengerCapacity,
                luggageCapacity,
                engine,
                transmissionType);
        }

        public Vehicle CreateElectric(
            string regNr,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            PassengerCarCategoryType passengerCarCategoryType,
            VehicleComfortType vehicleComfortType,
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            int batteryCapacityKwh,
            int rangeKm,
            TransmissionType transmissionType,
            CountryCode countryCode)
        {
            ValidateCommonFields(regNr, model, year, horsePower);

            if (batteryCapacityKwh <= 0)
                throw new ArgumentOutOfRangeException(nameof(batteryCapacityKwh));

            if (rangeKm <= 0)
                throw new ArgumentOutOfRangeException(nameof(rangeKm));

            var registrationPlate = new RegistrationPlate(regNr, countryCode);

            IEngine engine = new ElectricEngine(
                horsePower,
                batteryCapacityKwh,
                rangeKm);

            return new Car(
                registrationPlate,
                model,
                vehicleCategoryType,
                passengerCarCategoryType,
                vehicleComfortType, // Tillagt
                year,
                passengerCapacity,
                luggageCapacity,
                engine,
                transmissionType);
        }

        private void ValidateCommonFields(
            string regNr,
            VehicleModel model,
            int year,
            int horsePower)
        {
            _regNrValidator.ValidateRegNr(regNr);

            if (model?.Brand == null)
                throw new ArgumentException(
                    "Modellen måste ha ett kopplat bilmärke.",
                    nameof(model));

            _brandModelValidator.ValidateBrand(model.Brand.BrandName);
            _brandModelValidator.ValidateModel(model.ModelName);
            _yearValidator.ValidateYear(year);
            _horsePowerValidator.ValidateHorsePower(horsePower);
        }
    }
}