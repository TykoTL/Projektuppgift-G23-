namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleInspectionRepository
    {
        private readonly List<VehicleInspection> _inspections = new();

        public void Add(VehicleInspection inspection)
        {
            if (inspection == null)
                throw new ArgumentNullException(nameof(inspection));

            _inspections.Add(inspection);
        }

        public VehicleInspection? GetById(Guid id)
        {
            return _inspections.FirstOrDefault(
                inspection => inspection.VehicleInspectionId == id);
        }

        public IEnumerable<VehicleInspection> GetByVehicleId(Guid vehicleId)
        {
            return _inspections
                .Where(inspection => inspection.VehicleId == vehicleId)
                .OrderByDescending(inspection => inspection.InspectionDate)
                .ToList();
        }

        public IEnumerable<VehicleInspection> GetAll()
        {
            return _inspections.ToList();
        }
    }
}

