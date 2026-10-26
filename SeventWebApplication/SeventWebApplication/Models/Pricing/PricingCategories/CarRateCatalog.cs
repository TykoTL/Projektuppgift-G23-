using System;
using SeventWebApplication.Models.VehicleNS;

public class CarRateCatalog
{
    private decimal _baseFee = 150;
    
    private readonly Dictionary<PassengerCarCategoryType, decimal> _passengerCarCategoryRates = new()
    {
        [PassengerCarCategoryType.Hatchback] = 0.9m,
        [PassengerCarCategoryType.Sedan] = 1m,
        [PassengerCarCategoryType.StationWagon] = 1.1m,
        [PassengerCarCategoryType.SUV] = 1.2m,
    };
    
    private readonly Dictionary<VehicleComfortType, decimal> _vehicleComfortRates = new()
    {
        [VehicleComfortType.Economy] = 450m,
        [VehicleComfortType.Standard] = 650m,
        [VehicleComfortType.Premium] = 800m,
        [VehicleComfortType.Luxury] = 1200m,
    }; 

    public void SetPassengerCarCategoryRate(PassengerCarCategoryType category, decimal newRate)
    {
        if (!_passengerCarCategoryRates.ContainsKey(category))
        {
            throw new KeyNotFoundException("Invalid category");
        }

        if (newRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newRate), "Price cannot be negative");
        }

        _passengerCarCategoryRates[category] = newRate;
    }
    
    public void SetVehicleComfortRate(VehicleComfortType category, decimal newRate)
    {
        if (!_vehicleComfortRates.ContainsKey(category))
        {
            throw new KeyNotFoundException("Invalid category");
        }

        if (newRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newRate), "Price cannot be negative");
        }

        _vehicleComfortRates[category] = newRate;
    }

    public void SetBaseFee(decimal baseFee)
    {
        _baseFee = baseFee;
    }

    public decimal GetDailyPrice(PassengerCarCategoryType category, VehicleComfortType vehicleComfort)
    {
        return _vehicleComfortRates[vehicleComfort] * _passengerCarCategoryRates[category];
    }
    
    public decimal GetBaseFee()
    {
        return _baseFee;
    }
}
