namespace AutoriaBot.Services;

public class GoogleMapsService
{
    private readonly string _apiKey;
    private readonly HttpClient _http = new();

    public GoogleMapsService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<double> GetDistanceAsync(string fromCity, string toCity)
    {
        var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={fromCity}&destinations={toCity}&key={_apiKey}";
        var json = await _http.GetStringAsync(url);
        // TODO: Реальний парсинг відстані
        return 13.4;
    }
}
