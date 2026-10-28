using SeventWebApplication.Models.Pricing.PricingCategories;
using SeventWebApplication.Models.VehicleNS;

namespace SeventWebApplication.Models.Pricing;

public class PricingServices
{
    private readonly CarRateCatalog _carRateCatalog;
    private readonly VanRateCatalog _vanRateCatalog;
    private readonly BaseCosts _baseCosts;
    
    public PricingServices(CarRateCatalog carRateCatalog, VanRateCatalog vanRateCatalog,  BaseCosts baseCosts)
    {
        _vanRateCatalog = vanRateCatalog;
        _carRateCatalog = carRateCatalog;
        _baseCosts = baseCosts;
    }

    public decimal GetDailyRate(Vehicle vehicle)
    {
       return vehicle switch
        { 
           Car car =>_carRateCatalog.GetDailyPrice(car),
           
           Van van => _vanRateCatalog.GetDailyPrice(van),
           
           _=> throw new ArgumentOutOfRangeException(nameof(vehicle), vehicle, null)
        };
    }
    
    public decimal CalculateTotalPrice(Vehicle vehicle, int billableDays) //Ska fixa så att denna hämtar det totala addon-priset också
    {
        return GetDailyRate(vehicle) *  billableDays + _baseCosts.GetBaseFee();
    }
    
    public decimal CalculateLateFee(string bookingId, TimeSpan delay)
    {
        var hours = (decimal)Math.Ceiling(delay.TotalHours);
        return hours * _baseCosts.GetDepositFee();
    }
}