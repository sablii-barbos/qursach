namespace AutoriaBot.Models;

public class SearchFilters
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int YearFrom { get; set; }
    public int YearTo { get; set; }
    public string City { get; set; }
    public int? PriceTo { get; set; }
    public string Fuel { get; set; }
    public string Gearbox { get; set; }
    public int? MileageTo { get; set; }
    public string BodyType { get; set; }
}
