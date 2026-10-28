using SeventWebApplication.Models.Booking;

namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleDamageService
    {
        private readonly FleetManager _fleetManager;
        private readonly VehicleInspectionRepository _inspectionRepository;
        private readonly VehicleDamageRepository _damageRepository;
        private readonly BookingRepository _bookingRepository;

        public VehicleDamageService(
            FleetManager fleetManager,
            VehicleInspectionRepository inspectionRepository,
            VehicleDamageRepository damageRepository,
            BookingRepository bookingRepository)
        {
            _fleetManager = fleetManager;
            _inspectionRepository = inspectionRepository;
            _damageRepository = damageRepository;
            _bookingRepository = bookingRepository;
        }

        public bool RegisterDamage(VehicleDamage damage)
        {
            if (damage == null || string.IsNullOrWhiteSpace(damage.Description))
                return false;

            var vehicle = _fleetManager.GetVehicle(damage.VehicleId);
            if (vehicle == null)
                return false;

            // Inspektion är valfri, men om den anges måste den finnas och gälla samma bil
            if (damage.InspectionId.HasValue)
            {
                var inspection = _inspectionRepository.GetById(damage.InspectionId.Value);

                if (inspection == null || inspection.VehicleId != damage.VehicleId)
                    return false;
            }

            _damageRepository.Add(damage);

            // En bil med öppen skada ska inte kunna hyras ut
            vehicle.IsAvailable = false;

            return true;
        }

        // Hämtar alla skador för en viss bil
        public IEnumerable<VehicleDamage> GetDamagesByVehicle(Guid vehicleId)
        {
            return _damageRepository.GetByVehicleId(vehicleId);
        }

        //  Hämtar alla skador som är kopplade till en viss inspektion
        public IEnumerable<VehicleDamage> GetDamagesByInspection(Guid inspectionId)
        {
            return _damageRepository.GetByInspectionId(inspectionId);
        }

        //  Kollar om bilen har några öppna skador (dvs. skador som inte är åtgärdade)
        public bool HasOpenDamages(Guid vehicleId)
        {
            return _damageRepository.GetByVehicleId(vehicleId)
                .Any(d => !d.IsResolved);
        }

        // Kollar om bilen är uthyrd just nu
        public bool IsCurrentlyRented(Guid vehicleId)
        {
            return _bookingRepository.GetAll()
                .Any(b => b.Vehicle != null
                       && b.Vehicle.VehicleId == vehicleId
                       && b.Status == "Active");
        }

        // Åtgärdar en skada och uppdaterar bilens tillgänglighet om det inte finns några öppna skador kvar
        public bool ResolveDamage(Guid damageId)
        {
            var damage = _damageRepository.GetById(damageId);

            if (damage == null || damage.IsResolved)
                return false;

            damage.Resolve();

            // Bilen blir ledig först när alla skador är åtgärdade OCH den inte är uthyrd
            if (!HasOpenDamages(damage.VehicleId) && !IsCurrentlyRented(damage.VehicleId))
            {
                var vehicle = _fleetManager.GetVehicle(damage.VehicleId);
                if (vehicle != null)
                    vehicle.IsAvailable = true;
            }

            return true;
        }
    }
}