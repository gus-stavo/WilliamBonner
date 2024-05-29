using HtmlAgilityPack;

namespace WilliamBonner.Features.DailyMessage;

public class PhraseService
{
    public static string GetTodaysPhrase()
    {
        string phrase = GetPhraseFromWebsite();
        return FormatPhrase(phrase);
    }

    private static string GetPhraseFromWebsite()
    {
        try
        {
            var url = "https://frasedodia.net";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var node = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div/div/div[1]/div/div[1]/a");

            return node.InnerText.Trim();
        }
        catch (Exception)
        {
            return "Sem frase do dia hoje :(";
        }
    }

    private static string FormatPhrase(string phrase)
    {
        return $"**Frase do dia:** {phrase}";
    }
}