using SeventWebApplication.Models.VehicleNS;

    // <summary>
    // The VehicleAssignmentService class is responsible for managing the assignment of vehicles to vehicle families.
    // It provides methods to find matching vehicles for a given vehicle family and to assign vehicles to families.
    // </summary>

    public class VehicleAssignmentService
    {
        private readonly FleetManager _fleetManager;
        private readonly VehicleFamilyRepository _familyRepository;
        private readonly VehicleFamilyAssignmentRepository _assignmentRepository;

        public VehicleAssignmentService(
            FleetManager fleetManager,
            VehicleFamilyRepository familyRepository,
            VehicleFamilyAssignmentRepository assignmentRepository)
        {
            _fleetManager = fleetManager;
            _familyRepository = familyRepository;
            _assignmentRepository = assignmentRepository;
        }

        public IEnumerable<Vehicle> FindMatchingVehicles(Guid familyId)
        {
            var family = _familyRepository.GetById(familyId);

            if (family == null)
                return Enumerable.Empty<Vehicle>();

            return _fleetManager.GetAllVehicles()
                .Where(vehicle => family.Matches(vehicle))
                .Where(vehicle => _assignmentRepository
                .GetFamilyId(vehicle.VehicleId) == familyId)
                .ToList();
        }
        public bool AssignVehicle(Guid vehicleId, Guid familyId)
        {
            var family = _familyRepository.GetById(familyId);

            var vehicle = _fleetManager.GetVehicle(vehicleId);

            if (family == null || vehicle == null)
                return false;

            if (!family.Matches(vehicle))
                return false;

            _assignmentRepository.Assign(vehicleId, familyId);
            return true;
        }
    }


