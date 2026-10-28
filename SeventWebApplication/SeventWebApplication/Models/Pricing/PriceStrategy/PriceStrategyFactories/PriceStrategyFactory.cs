

using System;
using SeventWebApplication.Models.Pricing.PriceStrategy;

namespace SeventWebApplication.Models.Pricing.PriceStrategy.PriceStrategyFactories
{
public abstract class BasePriceStrategyFactory 
{
        protected abstract IBasePriceStrategy CreatePriceStrategy();
}

public class basePriceStrategyFactory //Ett simple-factory om vi vill ha en mindre komplex lösning 
{
    public IBasePriceStrategy ChoiceOfPriceStrategy(string choiceOfStrategy) => choiceOfStrategy.ToLower() switch
    {
        "standard" => new StandardBasePriceStrategy(),
        _ => new StandardBasePriceStrategy()

    };
}
}
