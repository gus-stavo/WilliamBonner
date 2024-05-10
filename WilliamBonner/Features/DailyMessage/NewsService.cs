using HtmlAgilityPack;

namespace WilliamBonner.Features.DailyMessage;

public class NewsService
{
    public static string GetNewsMessage()
    {
        List<string> news = GetNews();
        var formattedNews = FormatNews(news);

        return "📰 **Notícias** 📰\n" +
              $"*{formattedNews}*\n";
    }

    private static List<string> GetNews()
    {
        try
        {
            var url = "https://www.cnnbrasil.com.br";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var news = new List<string>();

            var latestNewsNode = doc.DocumentNode.SelectSingleNode("//h2[@class='home__title headline__primary_title']");
            var latestNews = latestNewsNode.InnerText.Trim().Replace("&quot;", @"""");
            news.Add(latestNews);

            var otherNewsNode = doc.DocumentNode.SelectNodes("//ul[@class='headline__primary_list']/li");

            if (otherNewsNode is not null)
            {
                foreach (var liNode in otherNewsNode)
                {
                    var newsDescription = liNode.InnerText.Trim().Replace("&quot;", @"""");
                    news.Add(newsDescription);
                }
            }

            return news;
        }
        catch (Exception)
        {
            return new List<string>();
        }
    }

    private static string FormatNews(List<string> news)
    {
        return string.Join("\n", news.Select(item => $"- {item}"));
    }
}
