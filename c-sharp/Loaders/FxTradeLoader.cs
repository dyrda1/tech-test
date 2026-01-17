using HmxLabs.TechTest.Models;
using System.Globalization;

namespace HmxLabs.TechTest.Loaders
{
    public class FxTradeLoader : ITradeLoader
    {
        private const char Seperator = '¬';

        public string? DataFile { get; set; }

        public IEnumerable<ITrade> LoadTrades()
        {
            var tradeList = new TradeList();

            LoadTradesFromFile(DataFile, tradeList);

            return tradeList;
        }

        private FxTrade CreateTradeFromLine(string line_)
        {
            var items = line_.Split(new[] { Seperator });
            var trade = new FxTrade(items[8].Trim(), items[0].Trim());
            trade.TradeDate = DateTime.Parse(items[1].Trim());
            trade.Instrument = items[2].Trim() + items[3].Trim();
            trade.Notional = double.Parse(items[4].Trim());
            trade.Rate = double.Parse(items[5].Trim());
            trade.Counterparty = items[7].Trim();
            trade.ValueDate = DateTime.Parse(items[6].Trim());

            return trade;
        }

        private void LoadTradesFromFile(string? filename_, ITradeReceiver tradeList_)
        {
            if (filename_ == null)
                throw new ArgumentNullException(nameof(filename_));

            using var stream = new StreamReader(filename_);

            var lineCount = 0;
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (lineCount < 2)
                {
                    lineCount++;
                    continue;
                }

                if (line.StartsWith("END", StringComparison.OrdinalIgnoreCase))
                    break;

                tradeList_.Add(CreateTradeFromLine(line));
                lineCount++;
            }
        }
    }
}