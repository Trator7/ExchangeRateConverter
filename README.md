# ExchangeRateConverter
Basic library to retrieve currencies exchange rates base on European Central Bank.

This library store rates data when the are fetched from the website to reduce call to the website and improve the performance after the first download it's done.
If data is older than 1 day then the data will be refresh.

All the tranformations that doesn't have EUR as origin or target are computed as follow:
1. Retrieve Original currency to EUR rates
1. Retrieve EUR to Target currency rates
1. Compute Orinal to Target based on previous rates.

If there is no rate in a specific date the previous closest one will be returned.

# Public classes
* ExchangeRateTool (static class)
* ExchangeRate

# Basic configuration
There are some static fields that can be modified to determine where to save rates file data and how many days should check before an error is raised when a determined date does not have rate data.

* **ExchangeRateTool.DataDirectory**: Determine the folder where the file data will be stored. Default: "../DefaultExchangeRateConverter".
* **ExchangeRateTool.DataFileName**: Determine the data file name. Default: "ExchangeRateData.json".
* **ExchangeRateTool.DataFilePath**: combination of previous fields, you can directly update this field with full path and the other ones will be updated accordingly.

# ExchangeRateTool Static Class
Static class with main methods to retrieve and use exchange rates.

## ExchangeRateTool.GetCurrencyRates
### Description
Return a dictionary with all available rates for specific currencies
### Input arguments
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
### Return object
Dictionary<DateTime, double>: dictionary with date as Key and Rate as value
### Code Example
```
Dictionary<DateTime, double> jpyToIdr = ExchangeRateTool.GetCurrencyRates(CurrencyType.JPY, CurrencyType.IDR);
Console.WriteLine(jpyToIdr[new DateTime(2024, 1, 12)]);
```

## ExchangeRateTool.GetExchangeRateAtDate
### Description
Returns a rate from currencyFrom currency type to currencyTo type for specific date or the closest previous value.
### Input arguments
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
* DateTime Date: date of the rate to retrieve.
### Return object
double: specific exchange rate for a given date or nearest one.
### Code Example
```
double rate = ExchangeRateTool.GetExchangeRateAtDate(CurrencyType.USD, CurrencyType.GBP, new DateTime(2024, 1, 12));
Console.WriteLine(rate);
```

## ExchangeRateTool.ConvertAmount
### Description
Return a dictionary with all available rates for specific currencies
### Input arguments
* Amount: amount to convert by exchange rate.
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
* Date: date of the rate to use in the conversion.
### Return object
double: value of the converted amount.
### Code Example
```
double newAmount = ExchangeRateTool.ConvertAmount(100, CurrencyType.EUR, CurrencyType.USD, new DateTime(2023, 06, 06));
Console.WriteLine(newAmount);
```

## ExchangeRateTool.GetClosestRateToDate
### Description
Return the closets rate to the specified rate from the provieded rates
### Input arguments
* Dictionary<DateTime, double> rates: dictionary of dates and rates to search for closest rate.
* DateTime Date: date of the rate to retrieve.
### Return object
double: specific exchange rate for a given date or nearest one.
### Code Example
```
double rate = ExchangeRateTool.GetClosestRateToDate(ExchangeRateTool.GetCurrencyRates(CurrencyType.USD, CurrencyType.GBP), new DateTime(2024, 1, 1));
Console.WriteLine(rate);
```

# ExchangeRate Class
Class that keeps data about exchange rates between currencies.

## GetExchangeRate
### Description
Return the closets rate for the current ExchangeRate object given a specific date.
### Input arguments
* DateTime Date: date of the rate to retrieve.
### Return object
double: specific exchange rate for a given date or nearest one.
### Code Example
```
ExchangeRate eurToEur = new ExchangeRate(CurrencyType.EUR, CurrencyType.EUR);
Console.WriteLine(eurToEur.Rates.Count);
Console.WriteLine(eurToEur.GetExchangeRate(DateTime.Now));

ExchangeRate eurToUsd = new ExchangeRate(CurrencyType.EUR, CurrencyType.USD);
Console.WriteLine(eurToUsd.Rates.Count);
Console.WriteLine(eurToUsd.GetExchangeRate(new DateTime(2022, 8, 31)));
```
