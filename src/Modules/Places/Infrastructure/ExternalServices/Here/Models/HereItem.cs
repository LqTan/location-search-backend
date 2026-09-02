namespace Places.Infrastructure.ExternalServices.Here.Models;

internal sealed class HereItem
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public HerePosition Position { get; set; } = null!;
    public HereAddress? Address { get; set; }
    public List<HereCategory>? Categories { get; set; }
    public List<HereOpeningHours>? OpeningHours { get; set; }
}