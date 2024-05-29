using Discord.Commands;

namespace WilliamBonner.Commands;

public class CoinflipCommand : ModuleBase<SocketCommandContext>
{
    [Command("hat")]
    public async Task Coinflip()
    {
        await ReplyAsync(FlipCoin(1));
    }

    [Command("hat")]
    public async Task Coinflip(string qt)
    {
        if (!int.TryParse(qt, out int count) || count <= 0)
        {
            await ReplyAsync("Por favor, forneça um número inteiro positivo.");
            return;
        }

        await ReplyAsync(FlipCoin(count));
    }

    private static string FlipCoin(int count)
    {
        Random random = new Random();
        string[] results = new string[count];
        for (int i = 0; i < count; i++)
        {
            results[i] = random.Next(2) == 0 ? "Cara" : "Coroa";
        }

        return string.Join(" - ", results);
    }
}