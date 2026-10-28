namespace SeventWebApplication.Models.VehicleNS
{
    public class VehicleRentalRecordRepository
    {
        private readonly List<VehicleRentalRecord> _records = new();

        public void Add(VehicleRentalRecord record)
        {
            _records.Add(record);
        }

        public VehicleRentalRecord? GetById(Guid id)
        {
            return _records.FirstOrDefault(
                record => record.RentalRecordId == id);
        }

        public VehicleRentalRecord? GetByBookingId(string bookingId)
        {
            return _records.FirstOrDefault(
                record => record.BookingId == bookingId);
        }

        public void Update(VehicleRentalRecord record)
        {
            var index = _records.FindIndex(
                existing => existing.RentalRecordId == record.RentalRecordId);

            if (index < 0)
                throw new InvalidOperationException("Rental record not found.");

            _records[index] = record;
        }

        public IEnumerable<VehicleRentalRecord> GetAll()
        {
            return _records.ToList();
        }
    }
}