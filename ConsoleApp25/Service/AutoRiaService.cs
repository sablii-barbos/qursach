using System.Net.Http.Json;
using AutoriaBot.Models;

namespace AutoriaBot.Services;

public class AutoRiaService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public AutoRiaService(string apiKey)
    {
        _http = new HttpClient();
        _apiKey = apiKey;
    }

    public async Task<List<AutoSummary>> SearchAsync(SearchFilters filters)
    {
        // Моки для тесту. Замінити на API якщо потрібно.
        return new List<AutoSummary>
        {
            new AutoSummary
            {
                Title = $"{filters.Brand} {filters.Model} 2020",
                Price = 18500,
                City = filters.City ?? "Київ",
                Link = "https://auto.ria.com/auto123456.html",
                Description = "Стан нового авто, сервісна історія, всі ключі.",
                FuelType = filters.Fuel ?? "Бензин",
                Gearbox = filters.Gearbox ?? "Автомат",
                BodyType = filters.BodyType ?? "Седан",
                Mileage = 55000,
                PhotoUrls = new() { "https://cdn.pixabay.com/photo/2017/01/06/19/15/bmw-1957037_960_720.jpg" }
            }
        };
    }
}
