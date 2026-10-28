
using SeventWebApplication.Models.VehicleNS.Engine;

namespace SeventWebApplication.Models.VehicleNS
{
    public class Van : Vehicle,
        IPassengerVehicle,
        IVehicleTransmissionType,
        ICargoVehicle
    {
        public double CargoCapacityM3 { get; set; }
        public double MaxPayloadKg { get; set; }

        public VanCategoryType VanCategoryType { get; private set; }

        public Van(
            RegistrationPlate registrationPlate,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            VanCategoryType vanCategoryType,
            int year,
            int passengerCapacity,
            decimal luggageCapacity,
            IEngine engine,
            TransmissionType transmissionType,
            double cargoCapacityM3,
            double maxPayloadKg)
            : base(
                registrationPlate,
                model,
                vehicleCategoryType,
                year,
                passengerCapacity,
                luggageCapacity,
                engine,
                transmissionType)
        {
            VanCategoryType = vanCategoryType;
            CargoCapacityM3 = cargoCapacityM3;
            MaxPayloadKg = maxPayloadKg;
        }
    }
}
