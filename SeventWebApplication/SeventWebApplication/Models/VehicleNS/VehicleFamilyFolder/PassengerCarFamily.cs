namespace SeventWebApplication.Models.VehicleNS
{
    public class PassengerCarFamily : VehicleFamily, IPassengerCarFamily
    {
        public VehicleComfortType ComfortType { get; private set; }
        public PassengerCarCategoryType CategoryType { get; private set; }

        public PassengerCarFamily(
            Guid familyId,
            string name,
            string description,
            TransmissionType transmissionType,
            VehicleComfortType comfortType,
            PassengerCarCategoryType categoryType)
            : base(
                familyId,
                name,
                description,
                VehicleCategoryType.PassengerCar,
                transmissionType)
        {
            ComfortType = comfortType;
            CategoryType = categoryType;
        }

        public override bool Matches(Vehicle vehicle)
        {
            if (vehicle is not Car car)
                return false;

            return car.VehicleComfortType == ComfortType
                && car.PassengerCarCategoryType == CategoryType
                && car.TransmissionType == TransmissionType;
        }
    }
}
