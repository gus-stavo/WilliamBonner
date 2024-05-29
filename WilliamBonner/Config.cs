using Microsoft.Extensions.Configuration;

namespace WilliamBonner;

public static class Config
{
    public static string NewsApiKey { get; private set; } = "";
    public static string DiscordToken { get; private set; } = "";

    public static void Init(IConfigurationRoot configuration)
    {
        NewsApiKey = configuration["NewsApi:ApiKey"] ?? throw new ArgumentNullException("NewsApi:ApiKey");
        DiscordToken = configuration["Discord:Token"] ?? throw new ArgumentNullException("Discord:Token");
    }
}