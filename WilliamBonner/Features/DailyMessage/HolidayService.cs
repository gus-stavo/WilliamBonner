using Newtonsoft.Json;
using WilliamBonner.Models;

namespace WilliamBonner.Features.DailyMessage;

public class HolidayService
{
    public static string GetHolidaysMessage()
    {
        List<Holiday> holidaysToday = GetHolidaysToday();
        string formattedHolidays = FormatHolidays(holidaysToday);

        return "🥳 **Vamos comemorar o** 🥳\n" +
              $"{formattedHolidays}";
    }

    private static List<Holiday> GetHolidaysToday()
    {
        List<Holiday> holidays = GetHolidays();
        int dayNow = DateTime.Now.Day;
        int monthNow = DateTime.Now.Month;

        return holidays.Where(holiday =>
        {
            int holidayDay = Convert.ToInt32(holiday.Date.Substring(0, 2));
            int holidayMonth = Convert.ToInt32(holiday.Date.Substring(3, 2));

            return holidayDay == dayNow && holidayMonth == monthNow;
        }).ToList();
    }

    private static List<Holiday> GetHolidays()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utils/holidays.json");
        using var reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<List<Holiday>>(json)!;
    }

    private static string FormatHolidays(List<Holiday> holidays)
    {
        return string.Join("\n", holidays.Select(h => h.Name));
    }
}