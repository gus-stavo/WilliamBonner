namespace WilliamBonner.Features.DailyMessage;

public static class DailyMessageService
{
    public static async Task<string> GetDailyMessage()
    {
        string holidayMessage = HolidayService.GetHolidaysMessage();
        string currenciesMessage = CurrencyService.GetCurrenciesMessage();
        string todaysPhraseMessage = PhraseService.GetTodaysPhrase();
        string newsMessage = await NewsService.GetNewsMessage();
        string horoscopeMessage = ZodiacService.GetHoroscopeMessage();

        string weatherForecastMessage = WeatherForecastService.GetWeatherForecastMessage();

        var message = "**DELETE * FROM**\n" +
                     $"{DateTime.Now:dd/MM/yy}\n\n" +
                     $"{weatherForecastMessage}\n\n" +
                     $"{horoscopeMessage}\n\n" +
                     $"{todaysPhraseMessage}\n\n" +
                     $"{holidayMessage}\n\n" +
                     $"{currenciesMessage}\n\n" +
                     $"{newsMessage}\n";

        return message;
    }
}