using Search.Application.Abstractions;

namespace Search.Application.Search.Queries.SearchPlaces;

public sealed class SearchPlacesHandler
{
    private readonly IPlaceSearchService _placeSearchService;
    private readonly IRelevanceScorer _relevanceScorer;
    private readonly IRankingService _rankingService;
    public SearchPlacesHandler(
        IPlaceSearchService placeSearchService,
        IRelevanceScorer relevanceScorer,
        IRankingService rankingService
    )
    {
        _placeSearchService = placeSearchService;
        _relevanceScorer = relevanceScorer;
        _rankingService = rankingService;
    }
    public async Task<IReadOnlyList<SearchResult>> HandleAsync(
        SearchPlacesQuery query,
        CancellationToken cancellationToken
    )
    {
        var candidates = await _placeSearchService.SearchAsync(
            query.Query,
            query.Latitude,
            query.Longitude,
            query.RadiusKm,
            cancellationToken
        );
        var relevanceScores = await _relevanceScorer.ScoreAsync(
            query.Query,
            candidates,
            cancellationToken
        );
        var scoreByPlaceId = relevanceScores.ToDictionary(
            x => x.PlaceId,
            x => x.Score
        );

        
        return candidates
            .Select(place =>
            {
                var relevanceScore = scoreByPlaceId.GetValueOrDefault(place.Id);
                var distanceKm = _rankingService.CalculateDistanceKm(
                    query.Latitude,
                    query.Longitude,
                    place.Latitude,
                    place.Longitude
                );
                var finalScore = _rankingService.CalculateFinalScore(
                    relevanceScore,
                    distanceKm,
                    query.RadiusKm,
                    place.Rating
                );
                return new SearchResult(
                    place.Id,
                    place.Name,
                    place.Address,
                    place.Latitude,
                    place.Longitude,
                    place.Category,
                    relevanceScore,
                    distanceKm,
                    finalScore
                );
            })
            .OrderByDescending(x => x.FinalScore)
            .ToList();
    }
}