namespace WilliamBonner.Models;

public class WeatherForecast
{
    public Results results { get; set; }

    public override string ToString()
    {
        return $"{results.temp}º {results.description}. Min/Max: {results.forecast[0].min}º/{results.forecast[0].max}º";
    }
}

public class Results
{
    public int temp { get; set; }
    public string date { get; set; }
    public string time { get; set; }
    public string condition_code { get; set; }
    public string description { get; set; }
    public string currently { get; set; }
    public string cid { get; set; }
    public string city { get; set; }
    public string img_id { get; set; }
    public int humidity { get; set; }
    public double cloudiness { get; set; }
    public double rain { get; set; }
    public string wind_speedy { get; set; }
    public int wind_direction { get; set; }
    public string sunrise { get; set; }
    public string sunset { get; set; }
    public string condition_slug { get; set; }
    public string city_name { get; set; }
    public List<Forecast> forecast { get; set; }
}

public class Forecast
{
    public string date { get; set; }
    public string weekday { get; set; }
    public int max { get; set; }
    public int min { get; set; }
    public double cloudiness { get; set; }
    public double rain { get; set; }
    public int rain_probability { get; set; }
    public string wind_speedy { get; set; }
    public string description { get; set; }
    public string condition { get; set; }
}

public enum WeatherForecastCity
{
    Santos,
    Guaruja,
    PraiaGrande
}
