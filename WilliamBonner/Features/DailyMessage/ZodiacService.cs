using HtmlAgilityPack;

namespace WilliamBonner.Features.DailyMessage;

public class ZodiacService
{
    public static readonly string[] Zodiacs = { "virgem", "aries", "touro", "gemeos", "cancer", "leao", "libra", "escorpiao", "sagitario", "capricornio", "aquario", "peixes" };

    public static string GetHoroscopeMessage()
    {
        string randomZodiac = GetRandomZodiac();
        string formattedZodiac = FormatZodiac(randomZodiac);
        string horoscope = GetHoroscope(randomZodiac);

        return $"**Horoscopo de { formattedZodiac}:** { horoscope}";
    }

    private static string GetRandomZodiac()
    {
        return Zodiacs[new Random().Next(0, Zodiacs.Length)];
    }

    public static string FormatZodiac(string zodiac)
    {
        return zodiac switch
        {
            "virgem" => "Virgem ♍",
            "aries" => "Áries ♈",
            "touro" => "Touro ♉",
            "gemeos" => "Gêmeos ♊",
            "cancer" => "Câncer ♋",
            "leao" => "Leão ♌",
            "libra" => "Libra ♎",
            "escorpiao" => "Escorpião ♏",
            "sagitario" => "Sagitário ♐",
            "capricornio" => "Capricórnio ♑",
            "aquario" => "Aquário ♒",
            "peixes" => "Peixes ♓",
            _ => throw new Exception("Unknow zodiac"),
        };
    }

    public static string GetHoroscope(string zodiac)
    {
        var url = $"https://www.uol.com.br/universa/horoscopo/{zodiac}/horoscopo-do-dia";
        var web = new HtmlWeb();
        var doc = web.Load(url);

        var node = doc.DocumentNode.SelectSingleNode("//div[@id='diario']/div[@class='horoscope-open-content']/p");

        return node.InnerText;
    }
}