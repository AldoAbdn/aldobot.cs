using Discord;
using Discord.WebSocket;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;

namespace aldobot.Handlers
{
    internal class TikTokLiveHandler : IHandler
    {
        private TikTokLiveClient _tikTokLiveClient;
        private readonly DiscordSocketClient _discordClient;
        private ulong _channelId;

        public TikTokLiveHandler(DiscordSocketClient discordClient) {
            string userName = Environment.GetEnvironmentVariable("TIKTOK_USERNAME") ?? throw new InvalidOperationException("Username not found in environment variables.");
            _tikTokLiveClient = new TikTokLiveClient(userName, "");
            _discordClient = discordClient;
            ConnectEvents();
            string channelId = Environment.GetEnvironmentVariable("DISCORD_LIVE_CHANNEL_ID") ?? throw new InvalidOperationException("Channel ID not found in environment variables.");
            _channelId = ulong.Parse(channelId);
        }

        private void ConnectEvents()
        {
            _tikTokLiveClient.OnConnected += OnConnected;
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

        private async void OnConnected(TikTokLiveClient client, bool connected)
        {
            await SendMessage("Live Stream Started!");
        }

        private async void OnLiveEnded(TikTokLiveClient client, ControlMessage e)
        {
            await SendMessage("Live Stream Ended!");
        }

        private void SetupTikTokLiveClient()
        {
            string userName = Environment.GetEnvironmentVariable("TIKTOK_USERNAME") ?? throw new InvalidOperationException("Username not found in environment variables.");
            _tikTokLiveClient = new TikTokLiveClient(userName, "");
            ConnectEvents();
        }

        private async Task StartTikTok()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _tikTokLiveClient.RunAsync();
                    }
                    catch (Exception)
                    {
                        SetupTikTokLiveClient();
                        await Task.Delay(5000); // Wait before retrying
                    }
                }
            });
        }

        public async Task RunAsync()
        {
            await StartTikTok();
        }

        public Task SetupAsync()
        {
            return Task.CompletedTask;
        }
    }
}
