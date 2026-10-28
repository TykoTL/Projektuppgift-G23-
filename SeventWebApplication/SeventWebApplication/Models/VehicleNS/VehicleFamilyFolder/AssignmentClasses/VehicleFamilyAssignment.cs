namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleFamilyAssignment
    {
        public Guid VehicleId { get; private set; }
        public Guid FamilyId { get; private set; }

        public VehicleFamilyAssignment(Guid vehicleId, Guid familyId)
        {
            VehicleId = vehicleId;
            FamilyId = familyId;
        }
    
    }
}
