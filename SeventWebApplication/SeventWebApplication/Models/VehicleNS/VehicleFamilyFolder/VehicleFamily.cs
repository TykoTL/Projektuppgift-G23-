namespace SeventWebApplication.Models.VehicleNS
{
    public abstract class VehicleFamily
    {
        public Guid FamilyId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public VehicleCategoryType VehicleCategoryType { get; private set; }
        public TransmissionType TransmissionType { get; private set; }

        protected VehicleFamily(
            Guid familyId,
            string name,
            string description,
            VehicleCategoryType vehicleCategoryType,
            TransmissionType transmissionType)
        {
            FamilyId = familyId;
            Name = name;
            Description = description;
            VehicleCategoryType = vehicleCategoryType;
            TransmissionType = transmissionType;
        }

        public abstract bool Matches(Vehicle vehicle);
    }
}
