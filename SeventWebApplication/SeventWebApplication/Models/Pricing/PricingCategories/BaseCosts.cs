namespace SeventWebApplication.Models.Pricing.PricingCategories;

public class BaseCosts
{
    private decimal _baseFee = 100;
    
    public decimal GetBaseFee()
    {
        return _baseFee;
    }

    private decimal _depositFee = 100;
    
    public decimal GetDepositFee()
    {
        return _depositFee; 
    }
    public void setDepositFee(decimal value)
    {
        _depositFee = value;
    }
}