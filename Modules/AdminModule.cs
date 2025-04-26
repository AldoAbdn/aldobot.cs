using Discord;
using Discord.Commands;

namespace aldobot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("ban")]
        [Summary("Ban a user")]
        public async Task Ban(IGuildUser user, string reason = "", int days = 0)
        {
            await Context.Guild.AddBanAsync(user, days, reason);
            await ReplyAsync($"Banned {user}");
        }

        [Command("unban")]
        [Summary("Unban a user")]
        public async Task Unban(IGuildUser user)
        {
            await Context.Guild.RemoveBanAsync(user);
            await ReplyAsync($"Unbanned {user}");
        }
    }
}
