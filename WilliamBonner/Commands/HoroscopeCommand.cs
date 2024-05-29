using Discord.Commands;
using WilliamBonner.Features.DailyMessage;

namespace WilliamBonner.Commands;

public class HoroscopeCommand : ModuleBase<SocketCommandContext>
{
    [Command("horoscopo")]
    public async Task Horoscope(string sign)
    {
        var formattedZodiac = ZodiacService.FormatZodiac(sign);
        var horoscope = ZodiacService.GetHoroscope(sign);

        var message = $"**DELETE * FROM**" +
                      $"\n{DateTime.Now:dd/MM/yy}\n\n" +
                      $"**Horoscopo de {formattedZodiac}:** {horoscope}\n";

        await ReplyAsync($"{message}");
    }
}