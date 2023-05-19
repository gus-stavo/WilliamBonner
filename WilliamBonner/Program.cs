using Discord;
using RestSharp;
using WilliamBonner;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Discord.WebSocket;
using System.Globalization;
using Microsoft.Extensions.Configuration;

public class Program
{
    private readonly static DiscordSocketClient _client = new DiscordSocketClient();
    private readonly static IConfigurationRoot _config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
    //teste
    public static Task Main() => new Program().MainAsync();

    public async Task MainAsync()
    {
        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, _config["Discord:Token"]);
        await _client.StartAsync();
        await SendDailyMessage();
    }

    private async static Task SendDailyMessage()
    {
        string dailyMessage = GetDailyMessage();

        ulong id = 854378830957641779;
        var channel = _client.GetChannel(id) as IMessageChannel;
        await channel!.SendMessageAsync(dailyMessage);
    }

    private static string GetDailyMessage()
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
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "holidays.json");
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


    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}