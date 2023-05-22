using Discord;
using RestSharp;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Discord.WebSocket;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using WilliamBonner.Models;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public class Program
{
    public static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

    private DiscordSocketConfig _discordConfig;
    private DiscordSocketClient _client;
    private CommandService _commands;
    private IServiceProvider _services;
    private IConfigurationRoot _config;

    public async Task MainAsync()
    {
        _discordConfig = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

        _client = new DiscordSocketClient(_discordConfig);
        _commands = new CommandService();

        _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .BuildServiceProvider();

        _client.Log += Log;

        await RegisteCommandsAsync();
        await _client.LoginAsync(TokenType.Bot, _config["Discord:Token"]);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }

    public async Task RegisteCommandsAsync() 
    {
        _client.MessageReceived += HandleCommandAsync;
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private async Task HandleCommandAsync(SocketMessage arg) 
    {
        var message = arg as SocketUserMessage;
        var context = new SocketCommandContext(_client, message);
        if (message.Author.IsBot) return;

        int argPos = 0;
        if (message.HasStringPrefix("-", ref argPos)) 
        {
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
        }
    }

    #region dailyMessage
    public static string GetDailyMessage()
    {
        List<Holiday> holidaysToday = GetHolidaysToday();
        string formattedHolidays = FormatHolidays(holidaysToday);

        Currencies currencies = GetCurrencies();
        string formattedDollarCurrency = FormatCurrency(currencies.USDBRL);
        string formattedEuroCurrency = FormatCurrency(currencies.EURBRL);

        string todaysPhrase = GetTodaysPhrase();

        List<string> news = GetNews();
        string formattedNews = FormatNews(news);

        string randomZodiac = GetRandomZodiac();
        string formattedZodiac = FormatZodiac(randomZodiac);
        string horoscope = GetHoroscope(randomZodiac);

        WeatherForecast santosWeatherForecast = GetWeatherForecast(WeatherForecastCity.Santos);
        WeatherForecast guarujaWeatherForecast = GetWeatherForecast(WeatherForecastCity.Guaruja);
        WeatherForecast praiaGrandeWeatherForecast = GetWeatherForecast(WeatherForecastCity.PraiaGrande);

        var message = @$"**DELETE * FROM**
{DateTime.Now:dd/MM/yy}

**:white_sun_cloud: Clima :cloud_rain:**
Santos: {santosWeatherForecast}
Guarujá: {guarujaWeatherForecast}
Praia Grande: {praiaGrandeWeatherForecast}

**Horoscopo de {formattedZodiac}:** {horoscope}

**Frase do dia:** {todaysPhrase}

:partying_face: **Vamos comemorar o** :partying_face:
{formattedHolidays}

:moneybag: **Economia** :moneybag:
Dólar: {formattedDollarCurrency}
Euro: {formattedEuroCurrency}

:newspaper: **Notícias** :newspaper:
*{formattedNews}*
";

        return message;
    }

    private static List<Holiday> GetHolidays()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utils/holidays.json");
        using var reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<List<Holiday>>(json)!;
    }

    private static List<Holiday> GetHolidaysToday()
    {
        List<Holiday> holidays = GetHolidays();

        return holidays.Where(holiday =>
        {
            int dayNow = DateTime.Now.Day;
            int monthNow = DateTime.Now.Month;

            int holidayDay = Convert.ToInt32(holiday.Date.Substring(0, 2));
            int holidayMonth = Convert.ToInt32(holiday.Date.Substring(3, 2));

            return holidayDay == dayNow && holidayMonth == monthNow;
        }).ToList();
    }

    private static string FormatHolidays(List<Holiday> holidays)
    {
        string formattedHolidays = "";

        foreach (var holiday in holidays)
        {
            formattedHolidays += $"{holiday.Name}\n";
        }

        return formattedHolidays.Remove(formattedHolidays.Length - 1, 1);
    }

    private static string GetRandomZodiac()
    {
        string[] zodiacs = { "virgem", "aries", "touro", "gemeos", "cancer", "leao", "libra", "escorpiao", "sagitario", "capricornio", "aquario", "peixes" };
        return zodiacs[new Random().Next(0, zodiacs.Length)];
    }

    private static string FormatZodiac(string zodiac)
    {
        switch (zodiac)
        {
            case "virgem": return "Virgem :virgo:";
            case "aries": return "Áries :aries:";
            case "touro": return "Touro :taurus:";
            case "gemeos": return "Gêmeos :gemini:";
            case "cancer": return "Câncer :cancer:";
            case "leao": return "Leão :leo:";
            case "libra": return "Libra :libra:";
            case "escorpiao": return "Escorpião :scorpius:";
            case "sagitario": return "Sagitário :sagittarius:";
            case "capricornio": return "Capricórnio :capricorn:";
            case "aquario": return "Aquário :aquarius:";
            case "peixes": return "Peixes :pisces:";
            default: throw new Exception("Unknow zodiac");
        }
    }

    private static string GetHoroscope(string zodiac)
    {
        var url = $"https://www.uol.com.br/universa/horoscopo/{zodiac}/horoscopo-do-dia";
        var web = new HtmlWeb();
        var doc = web.Load(url);

        var node = doc
            .DocumentNode
            .SelectSingleNode("//div[@id='diario']/div[@class='horoscope-open-content']/p");

        return node.InnerText;
    }

    private static Currencies GetCurrencies()
    {
        var client = new RestClient("https://economia.awesomeapi.com.br/last/USD-BRL,EUR-BRL");
        var request = new RestRequest();
        var response = client.Execute(request);
        return JsonConvert.DeserializeObject<Currencies>(response.Content!)!;
    }

    private static string FormatCurrency(ICurrency currency)
    {
        float convertedVarBid = float.Parse(currency.varBid, CultureInfo.InvariantCulture);
        string formattedVarBid = (convertedVarBid * 100).ToString("N2");

        if (convertedVarBid > 0)
        {
            return $"{currency.bid} (+{formattedVarBid}%) :chart_with_upwards_trend:";
        }
        else
        {

            return $"{currency.bid} ({formattedVarBid}%) :chart_with_downwards_trend:";
        }
    }

    private static string GetTodaysPhrase()
    {
        var url = "https://frasedodia.net";
        var web = new HtmlWeb();
        var doc = web.Load(url);

        var node = doc
            .DocumentNode
            .SelectSingleNode("/html/body/div[3]/div/div/div/div[1]/div/div[1]/a");

        return node.InnerText.Trim();
    }

    private static List<string> GetNews()
    {
        var url = "https://www.cnnbrasil.com.br";
        var web = new HtmlWeb();
        var doc = web.Load(url);
        var news = new List<string>();

        var latestNewsNode = doc
            .DocumentNode
            .SelectSingleNode("//h2[@class='home__title headline__primary_title']");

        var latestNews = latestNewsNode.InnerText.Trim().Replace("&quot;", @"""");
        news.Add(latestNews);

        var otherNewsNode = doc
            .DocumentNode
            .SelectNodes("//ul[@class='headline__primary_list']/li");

        if (otherNewsNode != null)
        {
            foreach (var liNode in otherNewsNode)
            {
                var newsDescription = liNode.InnerText.Trim().Replace("&quot;", @"""");
                news.Add(newsDescription);
            }
        }

        return news;
    }

    private static string FormatNews(List<string> news)
    {
        string formattedNews = "";

        foreach (var item in news)
        {
            formattedNews += $"- {item}\n";
        }

        return formattedNews.Remove(formattedNews.Length - 1, 1);
    }

    private static WeatherForecast GetWeatherForecast(WeatherForecastCity city)
    {
        string woeid = "";

        switch (city)
        {
            case WeatherForecastCity.Santos: woeid = "455991"; break;
            case WeatherForecastCity.Guaruja: woeid = "455952"; break;
            case WeatherForecastCity.PraiaGrande: woeid = "455987"; break;
            default: break;
        }

        var client = new RestClient($"https://api.hgbrasil.com/weather?woeid={woeid}");
        var request = new RestSharp.RestRequest();
        var response = client.Execute(request);
        return JsonConvert.DeserializeObject<WeatherForecast>(response.Content!)!;
    }
    #endregion 
}