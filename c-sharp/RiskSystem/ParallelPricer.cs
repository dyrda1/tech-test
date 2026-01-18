using System.Reflection;
using HmxLabs.TechTest.Models;

namespace HmxLabs.TechTest.RiskSystem;

public class ParallelPricer
{
    private readonly Dictionary<string, IPricingEngine> _pricers = new();

    public void Price(IEnumerable<ITrade> trades_, IScalarResultReceiver resultReceiver_)
    {
        LoadPricers();

        Parallel.ForEach(
            trades_,
            new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
            trade =>
            {
                try
                {
                    if (!_pricers.ContainsKey(trade.TradeType))
                    {
                        resultReceiver_.AddError(trade.TradeId, "No Pricing Engines available for this trade type");
                        return;
                    }

                    var pricer = _pricers[trade.TradeType];
                    pricer.Price(trade, resultReceiver_);
                }
                catch (Exception ex)
                {
                    resultReceiver_.AddError(trade.TradeId, ex.Message);
                }
            });
    }

    private void LoadPricers()
    {
        _pricers.Clear();

        var configPath = Path.Combine("PricingConfig", "PricingEngines.xml");
        var pricingConfigLoader = new PricingConfigLoader { ConfigFile = configPath };
        var pricerConfig = pricingConfigLoader.LoadConfig();

        foreach (var configItem in pricerConfig)
        {
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Pricers", "bin",
                "Debug", "net8.0", "Pricers.dll"));
            var asm = Assembly.LoadFrom(path);
            var type = asm.GetType(configItem.TypeName!, throwOnError: true);
            var instance = Activator.CreateInstance(type!);

            if (instance is not IPricingEngine engine)
                throw new InvalidCastException($"{configItem.TypeName} does not implement IPricingEngine");

            _pricers[configItem.TradeType!] = engine;
        }
    }
}