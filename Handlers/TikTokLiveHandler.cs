using Discord;
using Discord.WebSocket;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;

namespace aldobot.Handlers
{
    internal class TikTokLiveHandler
    {
        private readonly TikTokLiveClient _tikTokLiveClient;
        private readonly DiscordSocketClient _discordClient;
        private ulong _channelId;

        public TikTokLiveHandler(TikTokLiveClient tikTokLiveClient, DiscordSocketClient discordClient) {
            _tikTokLiveClient = tikTokLiveClient;
            _discordClient = discordClient;
            string channelId = Environment.GetEnvironmentVariable("DISCORD_LIVE_CHANNEL_ID") ?? throw new InvalidOperationException("Channel ID not found in environment variables.");
            _channelId = ulong.Parse(channelId);
            Connect();
        }

        private void Connect()
        {
            _tikTokLiveClient.OnLiveIntro += OnLiveIntro;
            _tikTokLiveClient.OnLiveEnded += OnLiveEnded;
        }

        private async Task SendMessage(string message)
        {
            var channel = await _discordClient.GetChannelAsync(_channelId);
            if (channel is ITextChannel textChannel)
            {
                await textChannel.SendMessageAsync(message);
            }
        }

        private async void OnLiveIntro(TikTokLiveClient client, LiveIntro liveIntro)
        {
            await SendMessage("Live Stream Started!");
        }

        private async void OnLiveEnded(TikTokLiveClient client, ControlMessage e)
        {
            await SendMessage("Live Stream Ended!");
        }
    }
}
