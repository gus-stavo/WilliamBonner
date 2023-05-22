using System.Threading.Tasks;
using Discord.Commands;

namespace Commands.Commands
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
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

        #region hat
        [Command("hat")]
        public async Task Hat()
        {
            Random random = new Random();
            if (random.Next() % 2 == 0) await ReplyAsync("Cara");
            else await ReplyAsync("Coroa");
        }

        [Command("hat")]
        public async Task Hat(string qt)
        {
            string sResult = "";

            for (int i = 0; i < int.Parse(qt); i++)
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
