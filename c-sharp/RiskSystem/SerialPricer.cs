using System;
using System.Collections.Generic;
using System.Reflection;
using HmxLabs.TechTest.Models;

namespace HmxLabs.TechTest.RiskSystem
{
    public class SerialPricer
    {
        public void Price(IEnumerable<ITrade> trades_, IScalarResultReceiver resultReceiver_)
        {
            LoadPricers();

            foreach (var trade in trades_)
            {
                if (!_pricers.ContainsKey(trade.TradeType))
                {
                    resultReceiver_.AddError(trade.TradeId, "No Pricing Engines available for this trade type");
                    continue;
                }

                var pricer = _pricers[trade.TradeType];
                pricer.Price(trade, resultReceiver_);
            }
        }

        private void LoadPricers()
        {
            var pricingConfigLoader = new PricingConfigLoader { ConfigFile = @".\PricingConfig\PricingEngines.xml" };
            var pricerConfig = pricingConfigLoader.LoadConfig();

            foreach (var configItem in pricerConfig)
            {
                if (string.IsNullOrWhiteSpace(configItem.TradeType))
                    throw new InvalidOperationException("Pricing config item missing TradeType");

                if (string.IsNullOrWhiteSpace(configItem.Assembly))
                    throw new InvalidOperationException($"Pricing config item for {configItem.TradeType} missing Assembly");

                if (string.IsNullOrWhiteSpace(configItem.TypeName))
                    throw new InvalidOperationException($"Pricing config item for {configItem.TradeType} missing TypeName");

                var asm = Assembly.Load(configItem.Assembly);

                var pricerType = asm.GetType(configItem.TypeName, throwOnError: true);

                var instance = Activator.CreateInstance(pricerType!);
                if (instance is null)
                    throw new InvalidOperationException($"Failed to create instance of {configItem.TypeName}");

                if (instance is not IPricingEngine pricer)
                    throw new InvalidCastException($"{configItem.TypeName} does not implement IPricingEngine");

                _pricers[configItem.TradeType] = pricer;
            }
        }

        private readonly Dictionary<string, IPricingEngine> _pricers = new Dictionary<string, IPricingEngine>();
    }
}