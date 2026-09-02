using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Places.Application.Abstractions;
using Places.Domain.Entities;
using Places.Infrastructure.ExternalServices.Here.Models;

namespace Places.Infrastructure.Providers;

public sealed class HerePlaceProvider : IPlaceProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public HerePlaceProvider(
        HttpClient httpClient,
        IConfiguration configuration
    )
    {
        _httpClient = httpClient;
        _apiKey = configuration["Here:ApiKey"]
            ?? throw new InvalidOperationException("HERE API key is missing.");
    }
    public async Task<IReadOnlyList<Place>> SearchAsync(
        string query,
        double latitude,
        double longitude,
        double radiusKm,
        CancellationToken cancellationToken = default
    )
    {
        var radiusMeters = (int)(radiusKm * 1000);
        var url = 
            $"v1/discover" +
            $"?in=circle:{latitude},{longitude};r={radiusMeters}" +
            $"&q={Uri.EscapeDataString(query)}" +
            $"&limit=20" +
            $"&apiKey={_apiKey}";
        var result = await _httpClient.GetFromJsonAsync<HereResponse>(
            url,
            cancellationToken
        );
        return result?.Items
            .Where(x => x.Position is not null)
            .Select(x => new Place(
                externalId: x.Id,
                name: x.Title,
                latitude: x.Position!.Lat,
                longitude: x.Position.Lng,
                source: "HERE",
                address: x.Address?.Label,
                category: x.Categories?.FirstOrDefault()?.Name,
                openingHours: x.OpeningHours?.FirstOrDefault()?.Text?
                    .FirstOrDefault()
            ))
            .ToList() ?? [];
    }    
}