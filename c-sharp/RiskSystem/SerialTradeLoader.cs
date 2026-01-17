using HmxLabs.TechTest.Loaders;
using HmxLabs.TechTest.Models;

namespace HmxLabs.TechTest.RiskSystem
{
    public class SerialTradeLoader
    {
        public IEnumerable<ITrade> LoadTrades()
        {
            var loaders = GetTradeLoaders();

            foreach (var loader in loaders)
            {
                foreach (var trade in loader.LoadTrades())
                {
                    yield return trade;
                }
            }
        }

        private IEnumerable<ITradeLoader> GetTradeLoaders()
        {
            var loaders = new List<ITradeLoader>();
            ITradeLoader loader = new BondTradeLoader { DataFile = @"TradeData/BondTrades.dat" };
            loaders.Add(loader);

            loader = new FxTradeLoader { DataFile = @"TradeData/FxTrades.dat" };
            loaders.Add(loader);

            return loaders;
        }
    }
}