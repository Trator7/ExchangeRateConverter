using ExchangeRateConverter;
using ExchangeRateConverter.Entities;
using ExchangeRateConverter.Enum;

namespace ExchangeRateConverterTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEurToEurExchange()
        {
            ExchangeRate eurToEur = new ExchangeRate(CurrencyType.EUR, CurrencyType.EUR);
            Assert.IsTrue(eurToEur.Rates.Count == 0 && eurToEur.GetExchangeRate(DateTime.Now) == 1);
        }

        [Test]
        public void TestEurToUsdExchange()
        {
            ExchangeRate eurToUsd = new ExchangeRate(CurrencyType.EUR, CurrencyType.USD);
            Assert.IsTrue(eurToUsd.Rates.Count > 5000 && eurToUsd.GetExchangeRate(new DateTime(2022, 8, 31)) == 1);
        }

        [Test]
        public void TestUsdToGbpExchange()
        {
            ExchangeRate usdToGbp = new ExchangeRate(CurrencyType.USD, CurrencyType.GBP);
            Assert.IsTrue(usdToGbp.Rates.Count > 5000 && Math.Round(usdToGbp.GetExchangeRate(new DateTime(2024, 1, 12)), 4) == 0.7855);
        }

        [Test]
        public void TestUsdToGbpExchangeWithExchangeDataManagement()
        {
            Assert.IsTrue(Math.Round(ExchangeRateTool.GetExchangeRateAtDate(CurrencyType.USD, CurrencyType.GBP, new DateTime(2024, 1, 12)), 4) == 0.7855);
        }

        [Test]
        public void TestJpyToIdrExchangeWithExchangeDataManagement()
        {
            Dictionary<DateTime, double> jpyToIdr = ExchangeRateTool.GetCurrencyRates(CurrencyType.JPY, CurrencyType.IDR);
            Assert.IsTrue(Math.Round(jpyToIdr[new DateTime(2024, 1, 12)], 2) == 107.04);
        }

        [Test]
        public void TestConvertAmountFromUsdToEur()
        {
            double newAmount = ExchangeRateTool.ConvertAmount(100, CurrencyType.EUR, CurrencyType.USD, new DateTime(2023, 06, 06));
            Assert.IsTrue(Math.Round(newAmount, 2) == 106.83);
        }

        [Test]
        public void TestClosestRateWhenEmpty()
        {
            double nonExistenRateDay = Math.Round(ExchangeRateTool.GetExchangeRateAtDate(CurrencyType.USD, CurrencyType.GBP, new DateTime(2024, 1, 1)), 4);
            double theoricalClosestDay = Math.Round(ExchangeRateTool.GetExchangeRateAtDate(CurrencyType.USD, CurrencyType.GBP, new DateTime(2023, 12, 29)), 4);
            Assert.IsTrue(nonExistenRateDay == theoricalClosestDay);
        }
    }
}