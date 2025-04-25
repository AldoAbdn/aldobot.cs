using aldobot.Handlers;
using Discord;
using Discord.WebSocket;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Client.Config;

internal class Program
{
    private static DiscordSocketClient? _discordClient;
    private static DiscordHandler? _discordHandler;
    private static TikTokLiveClient? _tikTokLiveClient;
    private static TikTokLiveHandler? _tiktokHandler;
    private static ClientSettings _tikTokClientSettings = new ClientSettings();

    private static async Task Main()
    {
        // Discord
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };
        _discordClient = new DiscordSocketClient(config);
        // TikTok
        SetupTikTokLiveClient();
        // Setup
        await SetupHandlers(_discordClient, _tikTokLiveClient);
        await Run(_discordClient, _tikTokLiveClient);
        // Delay to keep the program running
        await Task.Delay(-1);
    }

    private static async Task SetupHandlers(DiscordSocketClient discordClient, TikTokLiveClient tikTokLiveClient)
    {
        _discordHandler = new DiscordHandler(discordClient, new Discord.Commands.CommandService());
        await _discordHandler.InstallCommandsAsync();
        _tiktokHandler = new TikTokLiveHandler(tikTokLiveClient, discordClient);
    }

    private static void SetupTikTokLiveClient()
    {
        _tikTokClientSettings.SkipRoomInfo = true;
        _tikTokClientSettings.RetryOnConnectionFailure = true;
        string userName = Environment.GetEnvironmentVariable("TIKTOK_USERNAME") ?? throw new InvalidOperationException("Username not found in environment variables.");
        _tikTokLiveClient = new TikTokLiveClient(userName, "");
    }

    private static async Task StartTikTok()
    {
        await Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    await _tikTokLiveClient?.RunAsync();
                }
                catch (Exception)
                {
                    SetupTikTokLiveClient();
                    await Task.Delay(5000); // Wait before retrying
                }
            }
        });
    }

    private static async Task Run(DiscordSocketClient discordClient, TikTokLiveClient tikTokLiveClient)
    {
        string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? throw new InvalidOperationException("Token not found in environment variables.");
        await discordClient.LoginAsync(TokenType.Bot, token);
        await discordClient.StartAsync();
        await StartTikTok();
    }
}