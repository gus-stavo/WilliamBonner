namespace WilliamBonner.Features.DailyMessage;

public static class DailyMessageService
{
    public static string GetDailyMessage()
    {
        string holidayMessage = HolidayService.GetHolidaysMessage();
        string currenciesMessage = CurrencyService.GetCurrenciesMessage();
        string todaysPhrase = PhraseService.GetTodaysPhrase();
        string news = NewsService.GetNewsMessage();
        string horoscopeMessage = ZodiacService.GetHoroscopeMessage();

        string weatherForecastMessage = WeatherForecastService.GetWeatherForecastMessage();

        var message = "**DELETE * FROM**\n" +
                     $"{DateTime.Now:dd/MM/yy}\n\n" +
                     $"{weatherForecastMessage}\n\n" +
                     $"{horoscopeMessage}\n\n" +
                     $"{todaysPhrase}\n\n" +
                     $"{holidayMessage}\n\n" +
                     $"{currenciesMessage}\n\n" +
                     $"{news}\n";

        return message;
    }
}
