using Discord.Commands;
using WilliamBonner.Features.DailyMessage;

namespace WilliamBonner.Commands;

public class DailyCommand : ModuleBase<SocketCommandContext>
{
    [Command("daily")]
    public async Task Daily()
    {
        await ReplyAsync(await DailyMessageService.GetDailyMessage());
    }
}