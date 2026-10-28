using SeventWebApplication.Models.Booking;
using BookingEntity = SeventWebApplication.Models.Booking.Booking;

namespace SeventWebApplication.Models.VehicleNS
{
    public class RentalReturnService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly VehicleRentalRecordRepository _recordRepository;
        private readonly VehicleInspectionRepository _inspectionRepository;
        private readonly VehicleDamageService _damageService;
        private readonly PricingService _pricingService;

        public RentalReturnService(
            BookingRepository bookingRepository,
            VehicleRentalRecordRepository recordRepository,
            VehicleInspectionRepository inspectionRepository,
            VehicleDamageService damageService,
            PricingService pricingService)
        {
            _bookingRepository = bookingRepository;
            _recordRepository = recordRepository;
            _inspectionRepository = inspectionRepository;
            _damageService = damageService;
            _pricingService = pricingService;
        }

        public void RegisterPickup(
            string bookingId,
            int mileage,
            decimal fuelLevel,
            decimal batteryLevel,
            string notes = "")
        {
            var booking = GetBooking(bookingId);
            var vehicleId = booking.Vehicle.VehicleId;

            if (_recordRepository.GetByBookingId(bookingId) != null)
                throw new InvalidOperationException("Pickup has already been registered.");

            if (booking.Status != "Approved")
                throw new InvalidOperationException("Booking must be approved before pickup.");

            if (!booking.Vehicle.IsAvailable)
                throw new InvalidOperationException("Vehicle is not available.");

            if (_damageService.HasOpenDamages(vehicleId))
                throw new InvalidOperationException("Vehicle has unresolved damages.");

            // 1. Pickup-inspektion
            var inspection = new VehicleInspection(
                vehicleId,
                DateTime.Now,
                InspectionType.Pickup,
                mileage,
                InspectionResult.Approved,
                notes);

            _inspectionRepository.Add(inspection);

            // 2. Rental record
            var record = new VehicleRentalRecord
            {
                BookingId = bookingId,
                ActualPickupDate = DateTime.Now,
                PickupMileage = mileage,
                PickupFuelLevel = fuelLevel,
                PickupBatteryLevel = batteryLevel
            };

            _recordRepository.Add(record);

            // 3. Uppdatera bil och bokning
            booking.Vehicle.IsAvailable = false;
            booking.Status = "Active";
        }

        public void RegisterReturn(
            string bookingId,
            int mileage,
            decimal? fuelLevel,
            decimal? batteryLevel,
            InspectionResult inspectionResult,
            IEnumerable<string>? damageDescriptions = null,
            string notes = "")
        {
            var booking = GetBooking(bookingId);
            var record = GetRecord(bookingId);
            var vehicleId = booking.Vehicle.VehicleId;

            if (record.IsReturned())
                throw new InvalidOperationException("Return has already been registered.");

            if (mileage < record.PickupMileage)
                throw new ArgumentException("Return mileage cannot be lower than pickup mileage.");

            // 1. Retur-inspektion
            var inspection = new VehicleInspection(
                vehicleId,
                DateTime.Now,
                InspectionType.Return,
                mileage,
                inspectionResult,
                notes);

            _inspectionRepository.Add(inspection);

            // 2. Registrera skador kopplade till inspektionen
            foreach (var description in damageDescriptions ?? Enumerable.Empty<string>())
            {
                var damage = new VehicleDamage
                {
                    VehicleId = vehicleId,
                    InspectionId = inspection.VehicleInspectionId,
                    Description = description
                };

                if (!_damageService.RegisterDamage(damage))
                    throw new InvalidOperationException($"Could not register damage: {description}");
            }

            // 3. Uppdatera rental record
            record.ActualReturnDate = DateTime.Now;
            record.ReturnMileage = mileage;
            record.ReturnFuelLevel = fuelLevel;
            record.ReturnBatteryLevel = batteryLevel;

            _recordRepository.Update(record);

            // 4. Bilen blir bara ledig om den saknar öppna skador
            booking.Vehicle.IsAvailable = !_damageService.HasOpenDamages(vehicleId);
            booking.Status = "Completed";
        }

        public decimal CalculateLateFee(string bookingId)
        {
            var delay = GetDelay(bookingId);

            return delay > TimeSpan.Zero
                ? _pricingService.CalculateLateFee(bookingId, delay)
                : 0m;
        }

        public bool IsLateReturn(string bookingId)
        {
            return GetDelay(bookingId) > TimeSpan.Zero;
        }

        private BookingEntity GetBooking(string bookingId)
        {
            return _bookingRepository.GetById(bookingId)
                ?? throw new InvalidOperationException("Booking not found.");
        }

        private VehicleRentalRecord GetRecord(string bookingId)
        {
            return _recordRepository.GetByBookingId(bookingId)
                ?? throw new InvalidOperationException("No pickup record exists for this booking.");
        }

        private TimeSpan GetDelay(string bookingId)
        {
            var record = _recordRepository.GetByBookingId(bookingId);
            if (record == null || !record.IsReturned())
                return TimeSpan.Zero;

            var booking = _bookingRepository.GetById(bookingId);
            if (booking == null)
                return TimeSpan.Zero;

            return record.CalculateDelay(booking.EndDate);
        }
    }


//placeholder tills tykos kommer in
public class PricingService
    {
        private const decimal LateFeePerHour = 100m;   // justera efter behov

        public decimal CalculateLateFee(string bookingId, TimeSpan delay)
        {
            var hours = (decimal)Math.Ceiling(delay.TotalHours);
            return hours * LateFeePerHour;
        }
    }
}