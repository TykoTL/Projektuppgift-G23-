namespace SeventWebApplication.Models.VehicleNS
{
        public class VehicleDamageRepository
        {
            private readonly List<VehicleDamage> _damages = new();

            public void Add(VehicleDamage damage)
            {
                _damages.Add(damage);
            }

            public VehicleDamage? GetById(Guid id)
            {
                return _damages.FirstOrDefault(d => d.DamageId == id);
            }

            public IEnumerable<VehicleDamage> GetByVehicleId(Guid vehicleId)
            {
                return _damages
                    .Where(d => d.VehicleId == vehicleId)
                    .ToList();
            }

            public IEnumerable<VehicleDamage> GetByInspectionId(Guid inspectionId)
            {
                return _damages
                    .Where(d => d.InspectionId == inspectionId)
                    .ToList();
            }

            public IEnumerable<VehicleDamage> GetAll()
            {
                return _damages.ToList();
            }
        }
}

