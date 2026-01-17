using System;

namespace HmxLabs.TechTest.Models
{
    public class FxTrade : BaseTrade
    {
        public FxTrade(string tradeId_, string tradeType_)
        {
            if (string.IsNullOrWhiteSpace(tradeId_))
                throw new ArgumentException("A valid non null, non empty trade ID must be provided");

            if (string.IsNullOrWhiteSpace(tradeType_))
                throw new ArgumentException("A valid trade type must be provided");

            TradeId = tradeId_;
            TradeType = tradeType_;
        }

        public const string FxSpotTradeType = "FxSpot";
        public const string FxForwardTradeType = "FxFwd";

        public override string TradeType { get; }

        public DateTime ValueDate { get; set; }
    }
}
