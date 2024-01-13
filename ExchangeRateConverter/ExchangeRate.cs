using System.Text;
using System.Text.Json.Serialization;

namespace ExchangeRateConverter
{
    public class ExchangeRate
    {
        public CurrencyType CurrencyTypeFrom { get; }
        public CurrencyType CurrencyTypeTo { get; }
        public Dictionary<DateTime, double> Rates { get; private set; }
        public DateTime UpdatedDate { get; set; }

        [JsonConstructor]
        public ExchangeRate(CurrencyType currencyTypeFrom, CurrencyType currencyTypeTo, Dictionary<DateTime, double> rates, DateTime updatedDate)
        {
            CurrencyTypeFrom = currencyTypeFrom;
            CurrencyTypeTo = currencyTypeTo;
            Rates = rates;
            UpdatedDate = updatedDate;
        }

        public ExchangeRate(CurrencyType currencyTypeFrom, CurrencyType currencyTypeTo)
        {
            CurrencyTypeFrom = currencyTypeFrom;
            CurrencyTypeTo = currencyTypeTo;
            Rates = CurrencyTypeFrom == CurrencyTypeTo ? new Dictionary<DateTime, double>() : ExchangeRateTool.GetCurrencyRates(CurrencyTypeFrom, CurrencyTypeTo);
            UpdatedDate = DateTime.Today;
        }

        public ExchangeRate(CurrencyType currencyTypeFrom, CurrencyType currencyTypeTo, Dictionary<DateTime, double> rates)
        {
            CurrencyTypeFrom = currencyTypeFrom;
            CurrencyTypeTo = currencyTypeTo;
            Rates = rates;
            UpdatedDate = DateTime.Today;
        }

        public void UpdateData(Dictionary<DateTime, double> rates)
        {
            Rates = rates;
            UpdatedDate = DateTime.Today;
        }

        public double GetExchangeRate(DateTime date)
        {
            return CurrencyTypeFrom == CurrencyTypeTo ? 1 : Rates[date.Date];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"|{"Date",-11}|{"Rate",10}|");
            if (CurrencyTypeFrom == CurrencyTypeTo)
            {
                sb.AppendLine($"|{"*",-11}|{$"{1:0.#####}",10}|");
            }
            else
            {
                foreach ((DateTime date, double ratio) in Rates)
                {
                    sb.AppendLine($"|{$"{date:yyyy-MM-dd}",-11}|{$"{ratio:0.#####}",10}|");
                }
            }

            return sb.ToString();
        }
    }
}
