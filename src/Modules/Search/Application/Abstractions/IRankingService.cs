namespace Search.Application.Abstractions;

public interface IRankingService
{
    double CalculateDistanceKm(
        double userLatitude,
        double userLongitude,
        double placeLatitude,
        double placeLongitude
    );
    double CalculateFinalScore(
        double relevanceScore,
        double distanceKm,
        double radiusKm,
        double? rating
    );
}