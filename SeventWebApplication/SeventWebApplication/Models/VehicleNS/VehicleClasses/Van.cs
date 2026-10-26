namespace SeventWebApplication.Models.VehicleNS
{
    public class Van : Vehicle,
        IPassengerVehicle,
        IFuelType,
        IVehicleTransmissionType,
        ICargoVehicle
    {
        public FuelType FuelType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public double CargoCapacityM3 { get; set; }
        public double MaxPayloadKg { get; set; }
        public VanCategoryType VanCategoryType  { get; set; }

        public Van(
            RegistrationPlate registrationPlate,
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
            VanCategoryType vanCategoryType)
            : base(
                registrationPlate,
                model,
                vehicleCategoryType,
                year,
                horsePower,
                passengerCapacity,
                luggageCapacity)
        {
            FuelType = fuelType;
            TransmissionType = transmissionType;
            CargoCapacityM3 = cargoCapacityM3;
            MaxPayloadKg = maxPayloadKg;
            VanCategoryType = vanCategoryType;
        }
    }
}