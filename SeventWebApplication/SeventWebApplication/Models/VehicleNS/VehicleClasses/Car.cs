namespace SeventWebApplication.Models.VehicleNS
{
    public class Car : Vehicle,
        IPassengerVehicle,
        IFuelType,
        IVehicleTransmissionType
    {
        public FuelType FuelType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public PassengerCarCategoryType PassengerCarCategoryType { get; set; }
        public VehicleComfortType VehicleComfortType { get; set; }

        public Car(
            RegistrationPlate registrationPlate,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            int year,
            int horsePower,
            int passengerCapacity,
            decimal luggageCapacity,
            FuelType fuelType,
            TransmissionType transmissionType,
            PassengerCarCategoryType passengerCarCategoryType,
            VehicleComfortType comfortType)
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
            PassengerCarCategoryType = passengerCarCategoryType;
            VehicleComfortType = comfortType;
        }
    }
}