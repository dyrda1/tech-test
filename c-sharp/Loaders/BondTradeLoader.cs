using HmxLabs.TechTest.Models;

namespace HmxLabs.TechTest.Loaders
{
    public class BondTradeLoader : ITradeLoader
    {
        private const char Seperator = ',';

        public IEnumerable<ITrade> LoadTrades()
        {
            return LoadTradesFromFile(DataFile);
        }

        public string? DataFile { get; set; }

        private BondTrade CreateTradeFromLine(string line_)
        {
            var items = line_.Split(new[] { Seperator });
            var trade = new BondTrade(items[6], items[0]);
            trade.TradeDate = DateTime.Parse(items[1]);
            trade.Instrument = items[2];
            trade.Counterparty = items[3];
            trade.Notional = Double.Parse(items[4]);
            trade.Rate = Double.Parse(items[5]);

            return trade;
        }

        private IEnumerable<ITrade> LoadTradesFromFile(string filename_)
        {
            if (null == filename_)
                throw new ArgumentNullException(nameof(filename_));

            using var stream = new StreamReader(filename_);

            var lineCount = 0;
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine();
                if (lineCount == 0)
                {
                    lineCount++;
                    continue;
                }

                yield return CreateTradeFromLine(line);
                lineCount++;
            }
        }
    }
}
