namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleBrand : IVehicleBrand
    {
        public Guid BrandId { get; private set; } = Guid.NewGuid();
        public string BrandName { get; private set; }

        public List<VehicleModel> Models { get; private set; } = new();

        public List<VehicleCategoryType> SupportedTypes { get; private set; } = new();

        public VehicleBrand(string name, params VehicleCategoryType[] types)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Brand name is required.", nameof(name));

            BrandName = name;
            SupportedTypes = types.ToList();
        }
    }
}
