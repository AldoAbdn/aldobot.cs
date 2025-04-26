using Discord;
using Discord.Commands;

namespace aldobot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("ban")]
        [Summary("Ban a user")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(IGuildUser user, string reason = "", int days = 0)
        {
            await Context.Guild.AddBanAsync(user, days, reason);
        }

        [Command("unban")]
        [Summary("Unban a user")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Unban(IGuildUser user)
        {
            await Context.Guild.RemoveBanAsync(user);
        }

        [Command("kick")]
        [Summary("Kick a user")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick(IGuildUser user, string reason = "")
        {
            await user.KickAsync(reason);
        }
    }
}
