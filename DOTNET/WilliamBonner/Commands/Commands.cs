using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Commands.Commands
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

        private DiscordSocketClient _client;

        public Commands(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command("help")]
        public async Task Help()
        {
            string helpMessage = @"**Comandos**
-ping 
-daily
-corno 
-hat (optional int)";

            await ReplyAsync(helpMessage);
        }

        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        [Command("daily")]
        public async Task Daily()
        {
            await ReplyAsync(Program.GetDailyMessage());
        }

        [Command("corno")]
        public async Task Corno()
        {
            var author = Context.User as SocketGuildUser;
            var authorChannel = author.VoiceChannel;

            var onlineUsers = _client.Guilds.SelectMany(g => g.Users).Where(u => !u.IsBot && u.VoiceChannel == authorChannel);
            var random = new Random();
            var winner = onlineUsers.ElementAt(random.Next(onlineUsers.Count()));
            await ReplyAsync($"O vencedor do sorteio é: {winner.DisplayName}!");
        }

        #region hat
        [Command("hat")]
        public async Task Hat()
        {
            Random random = new Random();
            if (random.Next() % 2 == 0) await ReplyAsync("Cara");
            else await ReplyAsync("Coroa");
        }

        [Command("hat")]
        public async Task Hat(string times)
        {
            string sResult = "";

            if (int.Parse(times) > 5) 
            {
                await ReplyAsync($"Parâmetro maior que o limite permitido. (5)");
                return;
            }

            for (int i = 0; i < int.Parse(times); i++)
            {
                Random random = new Random();
                if (random.Next() % 2 == 0) sResult += "Cara - ";
                else sResult += "Coroa - ";
            }

            await ReplyAsync($"{sResult.Substring(0, sResult.Length - 3)}");
        }
        #endregion
    }
}
