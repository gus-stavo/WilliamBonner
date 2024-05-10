using Discord.Commands;
using WilliamBonner.Features.DailyMessage;

namespace WilliamBonner.Commands;

internal class DailyCommand : ModuleBase<SocketCommandContext>
{
    [Command("daily")]
    public async Task Daily()
    {
        await ReplyAsync(DailyMessageService.GetDailyMessage());
    }
}
