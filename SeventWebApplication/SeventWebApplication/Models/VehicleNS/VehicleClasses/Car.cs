using SeventWebApplication.Models.VehicleNS.Engine;

namespace SeventWebApplication.Models.VehicleNS
{
    public class Car : Vehicle,
        IPassengerVehicle,
        IVehicleTransmissionType
    {

        public PassengerCarCategoryType PassengerCarCategoryType { get; private set; } // Ändrat: private set
        public VehicleComfortType VehicleComfortType { get; set; } // Tillagt

        public Car(
            RegistrationPlate registrationPlate,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            PassengerCarCategoryType passengerCarCategoryType, // Tillagt
            VehicleComfortType vehicleComfortType, // Tillagt
            int year,
            int passengerCapacity,
            decimal luggageCapacity,
            IEngine engine, // Ändrat: ersätter horsePower och fuelType
            TransmissionType transmissionType)
            : base(
                registrationPlate,
                model,
                vehicleCategoryType,
                year,
                passengerCapacity,
                luggageCapacity,
                engine, // Ändrat: skickar motorn till Vehicle
                transmissionType)
        {

            PassengerCarCategoryType = passengerCarCategoryType; // Tillagt
            VehicleComfortType = vehicleComfortType; // Tillagt
        }
    }
}