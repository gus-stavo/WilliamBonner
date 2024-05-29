using Discord.Commands;

namespace WilliamBonner.Commands;

public class PingCommand : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task Ping()
    {
        await ReplyAsync("Pong");
    }
}