using HmxLabs.TechTest.Models;

namespace HmxLabs.TechTest.RiskSystem
{
    public class ScreenResultPrinter
    {
        public void PrintResults(ScalarResults results_)
        {
            foreach (var result in results_)
            {
                var hasResult = result.Result.HasValue;
                var hasError = !string.IsNullOrWhiteSpace(result.Error);

                if (hasResult && hasError)
                {
                    Console.WriteLine($"{result.TradeId} : {result.Result} : {result.Error}");
                }
                else if (hasResult)
                {
                    Console.WriteLine($"{result.TradeId} : {result.Result}");
                }
                else if (hasError)
                {
                    Console.WriteLine($"{result.TradeId} : {result.Error}");
                }
            }
        }
    }
}