using aldobot.Handlers;
using Discord.WebSocket;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Client.Config;

internal class Program
{
    private static DiscordHandler _discordHandler;
    private static TikTokLiveHandler _tiktokHandler;

    static Program()
    {
        _discordHandler = new DiscordHandler();
        _tiktokHandler = new TikTokLiveHandler(_discordHandler.Client);
    }

    private static async Task Main()
    {
        // Setup
        await SetupHandlers();
        await Run();
        // Delay to keep the program running
        await Task.Delay(-1);
    }

    private static async Task SetupHandlers()
    {
        await _discordHandler.InstallCommandsAsync();
    }

    private static async Task Run()
    {
        await _discordHandler.RunAsync();
        await _tiktokHandler.RunAsync();
    }
}