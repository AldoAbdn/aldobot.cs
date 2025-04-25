using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace aldobot.Handlers
{
    internal class DiscordHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public DiscordHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
            _client.MessageReceived += HandleCommandAsync;
            _client.Log += Log;
        }

        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

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
    }
}
