namespace WilliamBonner.Models;

public class Currencies
{
    public USDBRL USDBRL { get; set; }
    public EURBRL EURBRL { get; set; }
}

public class USDBRL : ICurrency
{
    public string code { get; set; }
    public string codein { get; set; }
    public string name { get; set; }
    public string high { get; set; }
    public string low { get; set; }
    public string varBid { get; set; }
    public string pctChange { get; set; }
    public string bid { get; set; }
    public string ask { get; set; }
    public string timestamp { get; set; }
    public string create_date { get; set; }
}

public class EURBRL : ICurrency
{
    public string code { get; set; }
    public string codein { get; set; }
    public string name { get; set; }
    public string high { get; set; }
    public string low { get; set; }
    public string varBid { get; set; }
    public string pctChange { get; set; }
    public string bid { get; set; }
    public string ask { get; set; }
    public string timestamp { get; set; }
    public string create_date { get; set; }
}

public interface ICurrency
{
    public string code { get; set; }
    public string codein { get; set; }
    public string name { get; set; }
    public string high { get; set; }
    public string low { get; set; }
    public string varBid { get; set; }
    public string pctChange { get; set; }
    public string bid { get; set; }
    public string ask { get; set; }
    public string timestamp { get; set; }
    public string create_date { get; set; }
}