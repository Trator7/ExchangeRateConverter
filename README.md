# ExchangeRateConverter
Basic library to retrieve currencies exchange rates base on European Central Bank.

This library store rates data when the are fetched from the website to reduce call to the website and improve the performance after the first download it's done.
If data is older than 1 day then the data will be refresh.

All the tranformations that doesn't have EUR as origin or target are computed as follow:
1. Retrieve Original currency to EUR rates
1. Retrieve EUR to Target currency rates
1. Compute Orinal to Target based on previous rates.

If there is no rate in a specific date the previous closest one will be returned.

# Basic configuration
There are some static fields that can be modified to determine where to save rates file data and how many days should check before an error is raised when a determined date does not have rate data.

DataDirectory: Determine the folder where the file data will be stored. Default: "../DefaultExchangeRateConverter".
DataFileName: Determine the data file name. Default: "ExchangeRateData.json".
DataFilePath: combination of previous fields, you can directly update this field with full path and the other ones will be updated accordingly.

# Public methods to retrieve data

## GetCurrencyRates
### Description
Return a dictionary with all available rates for specific currencies
### Input arguments
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
### Return object
Dictionary<DateTime, double>: dictionary with date as Key and Rate as value
 
## GetExchangeRateAtDate
### Description
Returns a rate from currencyFrom currency type to currencyTo type for specific date or the closest previous value.
### Input arguments
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
* DateTime Date: date of the rate to retrieve.
### Return object
double: specific exchange rate for an specific date or closest previous one.

## ConvertAmount
### Description
Return a dictionary with all available rates for specific currencies
### Input arguments
* Amount: amount to convert by exchange rate.
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
* Date: date of the rate to use in the conversion.
### Return object
double: value of the converted amount.

## GetClosestRateToDate
### Description
Return the closets rate to the specified rate from the provieded rates
### Input arguments
* Dictionary<DateTime, double> rates: dictionary of dates and rates to search for closest rate.
* DateTime Date: date of the rate to retrieve.
### Return object
double: specific exchange rate for an specific date or closest previous one.
