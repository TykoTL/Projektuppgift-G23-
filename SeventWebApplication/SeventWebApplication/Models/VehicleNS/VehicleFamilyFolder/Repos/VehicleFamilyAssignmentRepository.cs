namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleFamilyAssignmentRepository
    {
        private readonly List<VehicleFamilyAssignment> _assignments = new();

        // Assigns a vehicle to a family. If the vehicle is already assigned to a family, it will be reassigned to the new family.
        public void Assign(Guid vehicleId, Guid familyId)
        {
            var existing = _assignments
                .FirstOrDefault(a => a.VehicleId == vehicleId);

            if (existing != null)
            {
                _assignments.Remove(existing);
            }

            _assignments.Add(
                new VehicleFamilyAssignment(vehicleId, familyId));
        }

        //  Removes the assignment of a vehicle to a family. If the vehicle is not assigned to any family, this method does nothing.
        public Guid? GetFamilyId(Guid vehicleId)
        {
            return _assignments
                .FirstOrDefault(a => a.VehicleId == vehicleId)
                ?.FamilyId;
        }

        // Returns all vehicle-family assignments.
        public IEnumerable<VehicleFamilyAssignment> GetAll()
        {
            return _assignments.ToList();
        }
    }
}

