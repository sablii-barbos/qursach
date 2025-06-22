namespace AutoriaBot.Models;

public class AutoSummary
{
    public string Title { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public string City { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public string FuelType { get; set; }
    public string Gearbox { get; set; }
    public string BodyType { get; set; }
    public int Mileage { get; set; }
    public List<string> PhotoUrls { get; set; }
    public string Vin { get; internal set; }
}
