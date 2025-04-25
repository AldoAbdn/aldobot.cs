using Discord.Commands;

namespace aldobot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("admin")]
        [Summary("Admin commands")]
        public async Task Admin()
        {
            await ReplyAsync("Admin commands");
        }

        [Command("ban")]
        [Summary("Ban a user")]
        public async Task Ban([Remainder] string user)
        {
            await ReplyAsync($"Banned {user}");
        }

        [Command("kick")]
        [Summary("Kick a user")]
        public async Task Kick([Remainder] string user)
        {
            await ReplyAsync($"Kicked {user}");
        }
    }
}
