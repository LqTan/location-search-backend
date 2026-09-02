using Places.Application.Abstractions;
using Places.Domain.Entities;

namespace Places.Application.Places.Queries.SearchPlaces;

public sealed class SearchPlacesHandler
{
    private readonly IPlaceProvider _placeProvider;
    private readonly IPlaceRepository _placeRepository;

    public SearchPlacesHandler(
        IPlaceProvider placeProvider,
        IPlaceRepository placeRepository
    )
    {
        _placeProvider = placeProvider;
        _placeRepository = placeRepository;
    }
    public async Task<IReadOnlyList<Place>> HandleAsync(
        SearchPlacesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var places = await _placeProvider.SearchAsync(
            query.Query,
            query.Latitude,
            query.Longitude,
            query.RadiusKm,
            cancellationToken
        );
        await _placeRepository.UpsertRangeAsync(
            places,
            cancellationToken
        );
        return places;        
    }
}