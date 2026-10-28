using System.Reflection.Metadata.Ecma335;
using SeventWebApplication.Models.Pricing.PricingCategories;
using SeventWebApplication.Models.VehicleNS;

namespace SeventWebApplication.Models.Pricing.PricingCategories;

public class VanRateCatalog
{
    private readonly Dictionary<VanCategoryType, decimal> _vanTypeRates = new()
    {
        [VanCategoryType.PassengerVan] = 1100,
        [VanCategoryType.CargoVan] = 1400m
    };
    public void SetVanTypeRate(VanCategoryType category, decimal newRate)
    {
        if (!_vanTypeRates.ContainsKey(category))
        {
            throw new KeyNotFoundException("Invalid category");
        }

        if (newRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newRate), "Price cannot be negative");
        }

        _vanTypeRates[category] = newRate;
    }

    public decimal GetDailyPrice(Van van)
    {
        return _vanTypeRates[van.VanCategoryType];
    }
}