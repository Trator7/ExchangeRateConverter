# ExchangeRateConverter
Basic library to retrieve different currencies exchange rates base on European Central Bank.

All the tranformations that doesn't have EUR as origin or target are computed as follow:
1. Retrieve Original currency to EUR rates
1. Retrieve EUR to Target currency rates
1. Compute Orinal to Target based on previous rates.

# Public methods to retrieve data

## Dictionary<DateTime, double> GetCurrencyRates
### Description
Return a dictionary with all available rates for specific currencies
### Input arguments
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
### Return object
Dictionary<DateTime, double>: dictionary with date as Key and Rate as value
 
## GetExchangeRateAtDate(CurrencyType currencyFrom, CurrencyType currencyTo, DateTime date)
### Description
Returns a rate to transfrom from currencyFrom currency type to currencyTo type at specific date
### Input arguments
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
* Date: date of the rate to retrieve.
### Return object
double: specific exchange rate for an specific date

## ConvertAmount(double amount, CurrencyType currencyFrom, CurrencyType currencyTo)
ConvertAmount(double amount, CurrencyType currencyFrom, CurrencyType currencyTo, DateTime date)
### Description
Return a dictionary with all available rates for specific currencies
### Input arguments
* Amount: amount to convert by exchange rate.
* CurrencyType currencyFrom: original currency type.
* CurrencyType currencyTo: target currency type.
* Date: date of the rate to use in the conversion.
### Return object
double: value of the converted amount.
