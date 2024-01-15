using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using ExchangeRateConverter.Entities;
using ExchangeRateConverter.Enum;

namespace ExchangeRateConverter
{
    public static class ExchangeRateTool
    {
        public static string DataDirectory { get; set; } = "../DefaultExchangeRateConverter";
        public static string DataFileName { get; set; } = "ExchangeRateData.json";
        public static string DataFilePath
        { 
            get 
            {
                return Path.Combine(DataDirectory, DataFileName);
            } 
            set 
            {
                DataDirectory = Path.GetDirectoryName(value) ?? DataDirectory;
                DataFileName = Path.GetFileName(value) ?? DataFileName;
            }
        }

        public static int MaxNumberOfPreviousDaysBeforeError { get; set; } = 20;
        private static readonly Regex s_dateRegex = new Regex(@"\d+,\d+,\d+", RegexOptions.Compiled);
        private static readonly Regex s_rateRegex = new Regex(@"\d+\.?\d*", RegexOptions.Compiled);

        private static Dictionary<CurrencyType, ExchangeRate>? s_eurToCurrencyRateDict;
        private static Dictionary<CurrencyType, ExchangeRate> EurToCurrencyRateDict
        { 
            get
            {
                if(s_eurToCurrencyRateDict == null)
                {
                    s_eurToCurrencyRateDict = LoadCurrentDataAsJson();
                }
                return s_eurToCurrencyRateDict;
            }
        }

        public static Dictionary<DateTime, double> GetCurrencyRates(CurrencyType currencyFrom, CurrencyType currencyTo)
        {
            if(currencyFrom == CurrencyType.EUR)
            {
                return GetEurToTargetCurrencyRates(currencyTo);
            }
            else if (currencyTo == CurrencyType.EUR)
            {
                return GetOriginalCurrencyToEurRates(currencyFrom);
            }
            else
            {
                Dictionary<DateTime, double> originalCurrencyToEur = GetOriginalCurrencyToEurRates(currencyFrom);
                Dictionary<DateTime, double> eurToTargetCurrency = GetEurToTargetCurrencyRates(currencyTo);
                Dictionary<DateTime, double> computedRates = new Dictionary<DateTime, double>();

                foreach((DateTime date, double rate) in originalCurrencyToEur)
                {
                    if (eurToTargetCurrency.ContainsKey(date))
                    {
                        computedRates[date] = rate * eurToTargetCurrency[date];
                    }
                }

                return computedRates;
            }
        }

        public static double GetExchangeRateAtDate(CurrencyType currencyFrom, CurrencyType currencyTo, DateTime date)
        {
            return GetClosestRateToDate(GetCurrencyRates(currencyFrom, currencyTo), date.Date);
        }

        public static double ConvertAmount(double amount, CurrencyType currencyFrom, CurrencyType currencyTo)
        {
            return ConvertAmount(amount, currencyFrom, currencyTo, DateTime.Today);
        }

        public static double ConvertAmount(double amount, CurrencyType currencyFrom, CurrencyType currencyTo, DateTime date)
        {
            return amount * GetExchangeRateAtDate(currencyFrom, currencyTo, date);
        }

        public static double GetClosestRateToDate(Dictionary<DateTime, double> rates, DateTime date)
        {
            int daysBefore = 0;
            double rate = 0;

            do
            {
                if (rates.ContainsKey(date))
                {
                    rate = rates[date];
                }
                else
                {
                    daysBefore++;
                    date = date.AddDays(-1 * daysBefore);
                }
            } while (rate == 0 && daysBefore < MaxNumberOfPreviousDaysBeforeError);

            return rate;
        }

        private static Dictionary<DateTime, double> GetEurToTargetCurrencyRates(CurrencyType currencyType)
        {
            if (!EurToCurrencyRateDict.ContainsKey(currencyType))
            {
                EurToCurrencyRateDict[currencyType] = currencyType == CurrencyType.EUR ?
                    new ExchangeRate(CurrencyType.EUR, currencyType) :
                    new ExchangeRate(CurrencyType.EUR, currencyType, DownloadData(currencyType));

                SaveCurrentDataAsJson();
            }
            else if (EurToCurrencyRateDict[currencyType].UpdatedDate < DateTime.Today)
            {
                EurToCurrencyRateDict[currencyType].UpdateData(DownloadData(currencyType));

                SaveCurrentDataAsJson();
            }

            return EurToCurrencyRateDict[currencyType].Rates;
        }

        private static Dictionary<DateTime, double> GetOriginalCurrencyToEurRates(CurrencyType currencyType)
        {
            return GetEurToTargetCurrencyRates(currencyType).ToDictionary(k => k.Key, v => 1 / v.Value);
        }

        private static Dictionary<DateTime, double> DownloadData(CurrencyType currencyType)
        {
            HttpClient client = new HttpClient();

            string url = @$"https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/eurofxref-graph-{currencyType}.en.html?date={DateTime.Today:yyyy-MM-dd}".ToLower();
            Task<string> response = client.GetStringAsync(url);
            response.Wait();

            string webpageText = response.Result.ToString();
            Dictionary<DateTime, double> rates = new Dictionary<DateTime, double>();

            foreach (string line in webpageText.Split("\n"))
            {
                if (line.StartsWith("chartData.push("))
                {
                    string[] splitDate = s_dateRegex.Match(line).Value.Split(',');
                    string rateLineSection = line.Split("rate")[1];

                    DateTime dateTime = new DateTime(int.Parse(splitDate[0]), int.Parse(splitDate[1]) + 1, int.Parse(splitDate[2]));
                    double rate = double.Parse(s_rateRegex.Match(rateLineSection).Value, CultureInfo.InvariantCulture);
                    rates[dateTime] = rate;
                }
            }
            
            return rates;
        }

        private static void SaveCurrentDataAsJson()
        {
            File.WriteAllText(DataFilePath, JsonSerializer.Serialize(EurToCurrencyRateDict));
        }

        private static Dictionary<CurrencyType, ExchangeRate> LoadCurrentDataAsJson()
        {
            Directory.CreateDirectory(DataDirectory);
            return File.Exists(DataFilePath) ?
                JsonSerializer.Deserialize<Dictionary<CurrencyType, ExchangeRate>>(File.ReadAllText(DataFilePath)) ?? new Dictionary<CurrencyType, ExchangeRate>() :
                new Dictionary<CurrencyType, ExchangeRate>();
        }
    }
}