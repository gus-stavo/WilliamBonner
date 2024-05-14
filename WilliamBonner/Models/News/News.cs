namespace WilliamBonner.Models.News;

public class News
{
    public string status { get; set; }
    public int totalResults { get; set; }
    public List<Article> articles { get; set; }
}

public class Article
{
    public Source source { get; set; }
    public string author { get; set; }
    public string title { get; set; }
    public object description { get; set; }
    public string url { get; set; }
    public object urlToImage { get; set; }
    public DateTime publishedAt { get; set; }
    public object content { get; set; }
}

public class Source
{
    public string id { get; set; }
    public string name { get; set; }
}
