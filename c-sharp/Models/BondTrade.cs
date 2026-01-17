namespace HmxLabs.TechTest.Models
{
    public class BondTrade : BaseTrade
    {
        public BondTrade(string tradeId_, string tradeType_)
        {
            if (string.IsNullOrWhiteSpace(tradeId_))
            {
                throw new ArgumentException("A valid non null, non empty trade ID must be provided");
            }

            if (string.IsNullOrWhiteSpace(tradeType_))
            {
                throw new ArgumentException("A valid trade type must be provided");
            }

            TradeId = tradeId_;
            TradeType = tradeType_;
        }

        public const string GovBondTradeType = "GovBond";
        public const string CorpBondTradeType = "CorpBond";

        public override string TradeType { get; }
    }
}
