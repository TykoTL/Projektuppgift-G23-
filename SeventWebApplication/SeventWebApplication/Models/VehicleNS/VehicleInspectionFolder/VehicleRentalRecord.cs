using SeventWebApplication.Models.VehicleNS;
using BookingEntity = SeventWebApplication.Models.Booking.Booking;

namespace SeventWebApplication.Models.VehicleNS
{
    public record VehicleRentalRecord
    {
        public Guid RentalRecordId { get; set; } = Guid.NewGuid();

        public required string BookingId { get; set; }

        public DateTime ActualPickupDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        public int PickupMileage { get; set; }
        public int? ReturnMileage { get; set; }

        public decimal PickupFuelLevel { get; set; }
        public decimal PickupBatteryLevel { get; set; }

        public decimal? ReturnFuelLevel { get; set; }
        public decimal? ReturnBatteryLevel { get; set; }

        public bool IsReturned()
        {
            return ActualReturnDate.HasValue;
        }

        public TimeSpan CalculateDelay(DateTime plannedReturnDate)
        {
            if(!IsReturned())
                return TimeSpan.Zero;
            var delay = ActualReturnDate!.Value - plannedReturnDate;
            return delay > TimeSpan.Zero ? delay : TimeSpan.Zero;
        }

    }
}
