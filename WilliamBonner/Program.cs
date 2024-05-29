using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WilliamBonner;

public class Program
{
    public static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

    private DiscordSocketConfig _discordConfig;
    private DiscordSocketClient _client;
    private CommandService _commands;
    private IServiceProvider _services;
    private IConfigurationRoot _config;

    private async Task MainAsync()
    {
        _discordConfig = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        Config.Init(_config);

        _client = new DiscordSocketClient(_discordConfig);
        _commands = new CommandService();

        _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .BuildServiceProvider();

        _client.Log += Log;

        await RegisteCommandsAsync();
        await _client.LoginAsync(TokenType.Bot, Config.DiscordToken);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }

    private async Task RegisteCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        var context = new SocketCommandContext(_client, message);

        if (message.Author.IsBot)
            return;

        int argPos = 0;

        if (message.HasStringPrefix("-", ref argPos))
        {
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
        }
    }
}