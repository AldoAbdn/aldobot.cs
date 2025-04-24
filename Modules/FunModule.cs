using Discord.Commands;

namespace aldobot.Modules
{
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Ping!")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}
