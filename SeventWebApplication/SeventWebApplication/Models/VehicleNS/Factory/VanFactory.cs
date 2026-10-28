using SeventWebApplication.Models.VehicleNS.Engine;

namespace SeventWebApplication.Models.VehicleNS
{
    public class VanFactory : IVehicleFactory
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
            VanCategoryType vanCategoryType,
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            FuelType fuelType,
            TransmissionType transmissionType,
            double cargoCapacityM3,
            double maxPayloadKg,
            CountryCode countryCode)
        {
            ValidateCommonFields(regNr, model, year, horsePower);

            var registrationPlate = new RegistrationPlate(regNr, countryCode);

            IEngine engine = fuelType switch
            {
                FuelType.Petrol => new PetrolEngine(horsePower, 50),
                FuelType.Diesel => new DieselEngine(horsePower, 50),
                _ => throw new ArgumentException(
                    "Bränsletypen stöds inte av CreateStandard.",
                    nameof(fuelType))
            };

            return new Van(
                registrationPlate,
                model,
                vehicleCategoryType,
                vanCategoryType,
                year,
                passengerCapacity,
                luggageCapacity,
                engine,
                transmissionType,
                cargoCapacityM3,
                maxPayloadKg);
        }

        public Vehicle CreateElectric(
            string regNr,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            VanCategoryType vanCategoryType,
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            int batteryCapacityKwh,
            int rangeKm,
            TransmissionType transmissionType,
            double cargoCapacityM3,
            double maxPayloadKg,
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

            return new Van(
                registrationPlate,
                model,
                vehicleCategoryType,
                vanCategoryType,
                year,
                passengerCapacity,
                luggageCapacity,
                engine,
                transmissionType,
                cargoCapacityM3,
                maxPayloadKg);
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