using Newtonsoft.Json;
using RestSharp;
using WilliamBonner.Models;

namespace WilliamBonner.Features.DailyMessage;

public class WeatherForecastService
{
    public static string GetWeatherForecastMessage()
    {
        WeatherForecast santosWeatherForecast = GetWeatherForecast(WeatherForecastCity.Santos);
        WeatherForecast guarujaWeatherForecast = GetWeatherForecast(WeatherForecastCity.Guaruja);
        WeatherForecast praiaGrandeWeatherForecast = GetWeatherForecast(WeatherForecastCity.PraiaGrande);

        return "**🌥️ Clima 🌧️**\n" +
              $"Santos: {santosWeatherForecast}\n" +
              $"Guarujá: {guarujaWeatherForecast}\n" +
              $"Praia Grande: {praiaGrandeWeatherForecast}";
    }

    private static WeatherForecast GetWeatherForecast(WeatherForecastCity city)
    {
        string woeid = GetWoeid(city);

        var client = new RestClient($"https://api.hgbrasil.com/weather?woeid={woeid}");
        var request = new RestRequest();
        var response = client.Execute(request);
        return JsonConvert.DeserializeObject<WeatherForecast>(response.Content!)!;
    }

    private static string GetWoeid(WeatherForecastCity city)
    {
        return city switch
        {
            WeatherForecastCity.Santos => "455991",
            WeatherForecastCity.Guaruja => "455952",
            WeatherForecastCity.PraiaGrande => "455987",
            _ => "",
        };
    }
}