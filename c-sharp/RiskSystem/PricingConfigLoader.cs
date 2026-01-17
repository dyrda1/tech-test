using System.Xml.Linq;

namespace HmxLabs.TechTest.RiskSystem
{
    public class PricingConfigLoader
    {
        public string? ConfigFile { get; set; }

        public PricingEngineConfig LoadConfig()
        {
            if (ConfigFile == null)
                throw new ArgumentNullException(nameof(ConfigFile));

            var config = new PricingEngineConfig();

            var doc = XDocument.Load(ConfigFile);

            foreach (var engineNode in doc.Descendants("Engine"))
            {
                var item = new PricingEngineConfigItem
                {
                    TradeType = (string?)engineNode.Attribute("tradeType"),
                    Assembly = (string?)engineNode.Attribute("assembly"),
                    TypeName = (string?)engineNode.Attribute("pricingEngine")
                };

                config.Add(item);
            }

            return config;
        }
    }
}