
using SeventWebApplication.Models.VehicleNS.Engine;

namespace SeventWebApplication.Models.VehicleNS
{
    public abstract class Vehicle
    {
        public Guid VehicleId { get; private set; } = Guid.NewGuid();

        public RegistrationPlate RegistrationPlate { get; private set; }

        public VehicleModel Model { get; private set; }

        public static VehicleCategoryType VehicleCategoryType { get; private set; }

        public TransmissionType TransmissionType { get; set; }

        // Fordonet har en motor.
        public IEngine Engine { get; private set; }

        public int Year { get; private set; }
        public int PassengerCapacity { get; private set; }
        public decimal LuggageCapacity { get; private set; }

        public bool IsAvailable { get; set; } = true;

        protected Vehicle(
            RegistrationPlate registrationPlate,
            VehicleModel model,
            VehicleCategoryType vehicleCategoryType,
            int year,
            int passengerCapacity,
            decimal luggageCapacity,
            IEngine engine,
            TransmissionType transmissionType)
        {
            VehicleId = Guid.NewGuid();

            RegistrationPlate = registrationPlate
                ?? throw new ArgumentNullException(nameof(registrationPlate));

            Model = model
                ?? throw new ArgumentNullException(nameof(model));

            Engine = engine
                ?? throw new ArgumentNullException(nameof(engine));

            VehicleCategoryType = vehicleCategoryType;
            Year = year;
            PassengerCapacity = passengerCapacity;
            LuggageCapacity = luggageCapacity;
            TransmissionType = transmissionType;
        }
    }
}
