namespace SeventWebApplication.Models.VehicleNS
{
    public interface IPassengerCarFamily
    {
        VehicleComfortType ComfortType { get; }

        PassengerCarCategoryType CategoryType { get; }
    }
}
