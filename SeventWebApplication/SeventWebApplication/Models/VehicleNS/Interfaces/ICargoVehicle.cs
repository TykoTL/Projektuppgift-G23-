namespace SeventWebApplication.Models.VehicleNS
{
    public interface ICargoVehicle
    {
        public double CargoCapacityM3 { get; set; }
        public double MaxPayloadKg { get;  set; }
    }
}
