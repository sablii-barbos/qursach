namespace AutoriaBot.Services;

public class VinDecoderService
{
    private readonly HttpClient _http = new();

    public async Task<string> DecodeVinAsync(string vin)
    {
        var url = $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevinvalues/{vin}?format=json";
        return await _http.GetStringAsync(url);
    }
}