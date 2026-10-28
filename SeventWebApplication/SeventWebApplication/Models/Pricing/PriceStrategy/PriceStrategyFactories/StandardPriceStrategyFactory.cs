using System;
using SeventWebApplication.Models.Pricing.PriceStrategy;

namespace SeventWebApplication.Models.Pricing.PriceStrategy.PriceStrategyFactories
{


    public class StandardBasePriceStrategyFactory : BasePriceStrategyFactory
{
    protected override IBasePriceStrategy CreatePriceStrategy() => new StandardBasePriceStrategy();
}

} 