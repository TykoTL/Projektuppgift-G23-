namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleDamage
    {
        public Guid DamageId { get; set; } = Guid.NewGuid();
        public Guid VehicleId { get; set; }
        public Guid? InspectionId { get; set; } = null; // valfritt


        public required string Description { get; set; }
        public DateTime ReportedAt { get; set; }
        public string? PhotoUrl { get; set; } = null; // valfritt

        public bool IsResolved { get; private set; }
        public DateTime? ResolvedAt { get; private set; } = null;
        
        public void Resolve()
        {
            IsResolved = true;
            ResolvedAt = DateTime.UtcNow;
        }
    }
}
