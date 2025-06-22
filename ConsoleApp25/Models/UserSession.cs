using AutoriaBot.Models;

namespace AutoriaBot.UserData;

public class UserSession
{
    public long UserId { get; set; }
    public string? State { get; set; }
    public SearchFilters CurrentFilters { get; set; } = new();
    public List<AutoSummary> History { get; set; } = new();
    public List<AutoSummary> SavedCars { get; set; } = new();
    public List<AutoSummary> ComparisonList { get; set; } = new();

    public void Reset() => State = null;
}