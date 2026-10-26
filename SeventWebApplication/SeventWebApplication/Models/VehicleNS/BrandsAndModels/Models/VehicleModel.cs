namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleModel : IVehicleModel
    {
        public Guid ModelId { get; private set; } = Guid.NewGuid();

        public string ModelName { get; private set; }

        // Foreign key till VehicleBrand
        public Guid BrandId { get; private set; }

        // Navigation property
        public required VehicleBrand Brand { get; set; }

        public List<VehicleCategoryType> SupportedTypes { get; private set; } = new();

        public VehicleModel(
            string name,
            Guid brandId,
            params VehicleCategoryType[] types)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(
                    "Model name is required.", nameof(name));

            ModelName = name;
            BrandId = brandId;
            SupportedTypes = types.ToList();
        }
    }
}