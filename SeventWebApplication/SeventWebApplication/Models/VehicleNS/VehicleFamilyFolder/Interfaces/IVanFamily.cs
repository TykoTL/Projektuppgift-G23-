namespace SeventWebApplication.Models.VehicleNS
{
    public interface IVanFamily
    {
        VanCategoryType CategoryType { get; }

        double? MinCargoVolumeM3 { get; }

        double? MinPayloadKg { get; }

        int? MinPassengerCapacity { get; }
    }
}
