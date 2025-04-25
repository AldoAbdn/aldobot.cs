using aldobot.Handlers;
using Discord;
using Discord.WebSocket;

internal class Program
{
    private static DiscordSocketClient? _client;
    private static DiscordHandler? _discordHandler;
    private static TiktokHandler? _tiktokHandler;

    private static async Task Main()
    {
        // Configure
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };
        _client = new DiscordSocketClient(config);
        // Setup
        await SetupHandlers(_client);
        await Login(_client);
        // Delay to keep the program running
        await Task.Delay(-1);
    }

    private static async Task SetupHandlers(DiscordSocketClient client)
    {
        _discordHandler = new DiscordHandler(client, new Discord.Commands.CommandService());
        await _discordHandler.InstallCommandsAsync();
    }

    private static async Task Login(DiscordSocketClient client)
    {
        string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? throw new InvalidOperationException("Token not found in environment variables.");
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
    }
}