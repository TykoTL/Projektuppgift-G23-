using SeventWebApplication.Models.VehicleNS;

namespace SeventWebApplication.Models.VehicleNS
{
    public class VanFactory : IElectricVehicleFactory
    {
        private readonly IRegNrValidator _regNrValidator;
        private readonly IBrandModelValidator _brandModelValidator;
        private readonly IYearValidator _yearValidator;
        private readonly IHorsePowerValidator _horsePowerValidator;

        public VanFactory(
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
            double cargoCapacityM3,
            double maxPayloadKg,
            CountryCode countryCode,
            VanCategoryType vanCategoryType)
        {
            // Validera indata
            _regNrValidator.ValidateRegNr(regNr);
            _brandModelValidator.ValidateBrand(model.Brand.BrandName);
            _brandModelValidator.ValidateModel(model.ModelName);
            _yearValidator.ValidateYear(year);
            _horsePowerValidator.ValidateHorsePower(horsePower);

            // Skapa registreringsnummer som ett value object
            var registrationPlate = new RegistrationPlate(regNr, countryCode);

            // Skapa och returnera sk�pbilen
            return new Van(
                registrationPlate,
                model,
                vehicleCategoryType,
                year,
                horsePower,
                passengerCapacity,
                luggageCapacity,
                fuelType,
                transmissionType,
                cargoCapacityM3,
                maxPayloadKg,
                vanCategoryType);
        }

        public Vehicle CreateElectric(
            Vehicle baseVehicle,
            int batteryCapacityKwh,
            int rangeKm)
        {
            if (baseVehicle is not Van van)
            {
                throw new ArgumentException(
                    "BaseVehicle m�ste vara en Van",
                    nameof(baseVehicle));
            }

            return new ElectricVan(
                van,
                batteryCapacityKwh,
                rangeKm);
        }
    }
}