namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleFamilyRepository
    {
        private readonly Dictionary<Guid, VehicleFamily> _families = new();


        // Adds a new vehicle family to the repository.

        public void Add(VehicleFamily family)
        {
            ArgumentNullException.ThrowIfNull(family);

            // Check if a family with the same ID already exists in the repository
            if (!_families.TryAdd(family.FamilyId, family))
            {
                throw new InvalidOperationException(
                    $"En fordonsfamilj med ID {family.FamilyId} finns redan.");
            }
        }


        // Removes a vehicle family from the repository by its ID.

        public IEnumerable<VehicleFamily> GetAll()
        {
            return _families.Values.ToList();
        }

        // Retrieves a vehicle family from the repository by its ID.  

        public VehicleFamily? GetById(Guid id)
        {
            return _families.TryGetValue(id, out var family)
                ? family
                : null;
        }
    }
}
