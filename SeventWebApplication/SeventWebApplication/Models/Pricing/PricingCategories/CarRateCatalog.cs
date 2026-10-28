using System.Reflection.Metadata.Ecma335;
using SeventWebApplication.Models.Pricing.PricingCategories;
using SeventWebApplication.Models.VehicleNS;

namespace SeventWebApplication.Models.Pricing.PricingCategories;

public class CarRateCatalog
{
    private readonly Dictionary<PassengerCarCategoryType, decimal> _carBodyTypeRates = new()
    {
        [PassengerCarCategoryType.Hatchback] = 0.9m,
        [PassengerCarCategoryType.Sedan] = 1m,
        [PassengerCarCategoryType.StationWagon] = 1.1m,
        [PassengerCarCategoryType.SUV] = 1.20m,
    };
    private readonly Dictionary<VehicleComfortType, decimal> _baseDayRates = new()
    {
        [VehicleComfortType.Economy] = 450m,
        [VehicleComfortType.Standard] = 650m,
        [VehicleComfortType.Premium] = 800m,
        [VehicleComfortType.Luxury] = 1200m,
    };
    public void SetBodyTypeRate(PassengerCarCategoryType category, decimal newRate)
    {
        if (!_carBodyTypeRates.ContainsKey(category))
        {
            throw new KeyNotFoundException("Invalid category");
        }

        if (newRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newRate), "Price cannot be negative");
        }

        _carBodyTypeRates[category] = newRate;
    }
    public void SetBaseDayRate(VehicleComfortType category, decimal newPrice)
    {
        if (!_baseDayRates.ContainsKey(category))
        {
            throw new KeyNotFoundException("Invalid category");
        }

        if (newPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price cannot be negative");
        }

        _baseDayRates[category] = newPrice;
    }

    public decimal GetDailyPrice(Car car)
    {
        return _baseDayRates[car.VehicleComfortType] * _carBodyTypeRates[car.PassengerCarCategoryType];
    }
}