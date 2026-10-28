
namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleInspection
    {
        public Guid VehicleInspectionId { get; set; } = Guid.NewGuid();
        public Guid VehicleId { get; private set; }

        public DateTime InspectionDate { get; set; }
        public InspectionType InspectionType { get; set; }

        public int Mileage { get; set; }
        public string Notes { get; set; } = string.Empty;
        public InspectionResult InspectionResult { get; set; }


        public VehicleInspection(
            Guid vehicleId,
            DateTime inspectionDate,
            InspectionType inspectionType,
            int mileage,
            InspectionResult inspectionResult,
            string notes = "")
        {
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("VehicleId saknas.", nameof(vehicleId));

            if (mileage < 0)
                throw new ArgumentOutOfRangeException(nameof(mileage));

            VehicleId = vehicleId;
            InspectionDate = inspectionDate;
            InspectionType = inspectionType;
            Mileage = mileage;
            InspectionResult = inspectionResult;
            Notes = notes ?? string.Empty;
        }

        public void AddDamage(VehicleDamage damage, VehicleDamageRepository repository)
        {
            if (damage == null)
                throw new ArgumentNullException(nameof(damage));

            damage.InspectionId = VehicleInspectionId;
            damage.VehicleId = VehicleId;
            repository.Add(damage);
        }

        public IEnumerable<VehicleDamage> GetDamages(VehicleDamageRepository repository)
        {
            return repository.GetByInspectionId(VehicleInspectionId);
        }

        public bool HasOpenDamages(VehicleDamageRepository repository)
        {
            return GetDamages(repository).Any(damage => !damage.IsResolved);
        }
    }
}
