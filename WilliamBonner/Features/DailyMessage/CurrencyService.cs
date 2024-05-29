using Newtonsoft.Json;
using RestSharp;
using System.Globalization;
using WilliamBonner.Models;

namespace WilliamBonner.Features.DailyMessage;

public class CurrencyService
{
    public static string GetCurrenciesMessage()
    {
        Currencies currencies = GetCurrencies();
        string dollarFormatted = FormatCurrency(currencies.USDBRL);
        string euroFormatted = FormatCurrency(currencies.EURBRL);

        return "💰 **Economia** 💰\n" +
              $"Dólar: {dollarFormatted}\n" +
              $"Euro: {euroFormatted}";
    }

    private static Currencies GetCurrencies()
    {
        var client = new RestClient("https://economia.awesomeapi.com.br/last/USD-BRL,EUR-BRL");
        var request = new RestRequest();
        var response = client.Execute(request);
        return JsonConvert.DeserializeObject<Currencies>(response.Content!)!;
    }

    private static string FormatCurrency(ICurrency currency)
    {
        float convertedVarBid = float.Parse(currency.varBid, CultureInfo.InvariantCulture);
        string formattedVarBid = (convertedVarBid * 100).ToString("N2");

        if (convertedVarBid > 0)
        {
            return $"{currency.bid} (+{formattedVarBid}%) 📈";
        }
        else
        {
            return $"{currency.bid} ({formattedVarBid}%) 📉";
        }
    }
}