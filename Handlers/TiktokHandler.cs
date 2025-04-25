using Discord.WebSocket;

namespace aldobot.Handlers
{
    internal class TiktokHandler
    {
        private readonly DiscordSocketClient _discordClient;

        public TiktokHandler(DiscordSocketClient discordClient) {
            _discordClient = discordClient;
        }
    }
}
