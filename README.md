# ExchangeRateConverter
Basic library to retrieve different currencies exchange rates base on European Central Bank.

All the tranformations that doesn't have EUR as origin or target are computed as follow:
1. Retrieve Original currency to EUR rates
1. Retrieve EUR to Target currency rates
1. Compute Orinal to Target based on previous rates.

If there is no rate in a specific date the previous closest one will be returned.

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

## GetClosestRateToDate(Dictionary<DateTime, double> rates, DateTime date)
### Description
Return the closets rate to the specified rate from the provieded rates
### Input arguments
* Dictionary<DateTime, double> rates: dictionary of dates and rates to search for closest rate.
* DateTime Date: date of the rate to retrieve.
### Return object
double: specific exchange rate for an specific date or closest previous one.