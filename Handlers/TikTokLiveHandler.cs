using Discord.WebSocket;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;

namespace aldobot.Handlers
{
    internal class TikTokLiveHandler : IHandler
    {
        private TikTokLiveClient _tikTokLiveClient;
        private readonly DiscordSocketClient _discordClient;
        private string userName;

        public TikTokLiveHandler(DiscordSocketClient discordClient) {
            userName = Environment.GetEnvironmentVariable("TIKTOK_USERNAME") ?? throw new InvalidOperationException("Username not found in environment variables.");
            _tikTokLiveClient = new TikTokLiveClient(userName, "");
            _discordClient = discordClient;
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            _tikTokLiveClient.OnConnected += OnConnected;
            _tikTokLiveClient.OnLiveEnded += OnLiveEnded;
        }

        private async Task SendMessage(string message)
        {
            foreach(var channel in _discordClient.Guilds.SelectMany(g => g.TextChannels).Where(c => c.Name.ToLower().Contains("live stream")))
            {
                await channel.SendMessageAsync(message);
            }
        }

        private async void OnConnected(TikTokLiveClient client, bool connected)
        {
            await SendMessage($"Live Stream Started! https://www.tiktok.com/@{userName}/live");
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
                        if (_tikTokLiveClient.Connected)
                        {
                            await Task.Delay(60000); // Wait before checking again
                            continue;
                        }
                        await _tikTokLiveClient.RunAsync();
                    }
                    catch (Exception)
                    {
                        SetupTikTokLiveClient();
                        await Task.Delay(60000); // Wait before retrying
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
