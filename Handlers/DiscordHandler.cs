using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace aldobot.Handlers
{
    internal class DiscordHandler : IHandler
    {
        public DiscordSocketClient Client { get; }
        private readonly CommandService _commands;

        public DiscordHandler()
        {
            _commands = new CommandService();
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All
            };
            Client = new DiscordSocketClient(config);
            Client.MessageReceived += HandleCommandAsync;
            Client.UserJoined += UserJoined;
            Client.Log += Log;
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            string welcomeMessage = $"Welcome @{user.Username} to {user.Guild.Name}!";
            SocketTextChannel? welcomeChannel = user.Guild.TextChannels.FirstOrDefault(x => x.Name.ToLower().StartsWith("welcome"));
            if (welcomeChannel != null)
            {
                await welcomeChannel.SendMessageAsync(welcomeMessage);
            }
            else
            {
                await user.Guild.DefaultChannel.SendMessageAsync(welcomeMessage);
            }
        }

        public async Task SetupAsync()
        {
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(Client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(Client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task RunAsync()
        {
            string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? throw new InvalidOperationException("Token not found in environment variables.");
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
        }
    }
}
