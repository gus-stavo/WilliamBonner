using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace WilliamBonner.Features.DailyMessage;

public class NewsService
{
    public static async Task<string> GetNewsMessage()
    {
        List<string> news = await GetNews();
        var formattedNews = FormatNews(news);

        return "📰 **Notícias** 📰\n" +
              $"*{formattedNews}*\n";
    }

    private static async Task<List<string>> GetNews()
    {
        try
        {
            var newsApiClient = new NewsApiClient(Config.NewsApiKey);
            var response = await newsApiClient.GetTopHeadlinesAsync(new TopHeadlinesRequest
            {
                Country = Countries.BR,
                PageSize = 3,
            });

            if (response.Status == Statuses.Error)
                return new List<string> { "Erro ao obter notícias." };

            if (response.Articles.Count == 0)
                return new List<string> { "Nenhuma notícia disponível" };

            var news = response.Articles.Select(a => a.Title).ToList();

            return news;
        }
        catch (Exception)
        {
            return new List<string> { "Erro ao obter notícias." };
        }
    }

    private static string FormatNews(List<string> news)
    {
        return string.Join("\n", news.Select(item => $"- {item}"));
    }
}