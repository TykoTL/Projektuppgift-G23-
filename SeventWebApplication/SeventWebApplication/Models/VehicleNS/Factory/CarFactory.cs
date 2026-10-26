namespace SeventWebApplication.Models.VehicleNS
{
    public class CarFactory : IElectricVehicleFactory
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
            _regNrValidator.ValidateRegNr(regNr);
            _brandModelValidator.ValidateBrand(model.Brand.BrandName);
            _brandModelValidator.ValidateModel(model.ModelName);
            _yearValidator.ValidateYear(year);
            _horsePowerValidator.ValidateHorsePower(horsePower);

            var registrationPlate = new RegistrationPlate(
                regNr,
                countryCode);

            return new Car(
                registrationPlate,
                model,
                vehicleCategoryType,
                year,
                horsePower,
                passengerCapacity,
                luggageCapacity,
                fuelType,
                transmissionType,
                passengerCarCategoryType,
                vehicleComfortType
                );
        }

        public Vehicle CreateElectric(
            Vehicle baseVehicle,
            int batteryCapacityKwh,
            int rangeKm)
        {
            if (baseVehicle is not Car car)
            {
                throw new ArgumentException(
                    "BaseVehicle m�ste vara en Car",
                    nameof(baseVehicle));
            }

            return new ElectricCar(
                car,
                batteryCapacityKwh,
                rangeKm);
        }
    }
}