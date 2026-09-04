using Search.Application.Abstractions;

namespace Search.Infrastructure.Services;

public sealed class RankingService : IRankingService
{
    public double CalculateDistanceKm(
        double userLatitude,
        double userLongitude,
        double placeLatitude,
        double placeLongitude
    )
    {
        const double earthRadiusKm = 6371;
        double dLat = ToRadians(placeLatitude - userLatitude);
        double dLon = ToRadians(placeLongitude - userLongitude);

        double lat1 = ToRadians(userLatitude);
        double lat2 = ToRadians(placeLatitude);

        double a = 
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1) * Math.Cos(lat2) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(
            Math.Sqrt(a),
            Math.Sqrt(1 - a)
        );
        return earthRadiusKm * c;
    }
    public double CalculateFinalScore(
        double relevanceScore,
        double distanceKm,
        double radiusKm,
        double? rating
    )
    {
        double distanceScore =
            Math.Max(0, 1 - distanceKm / radiusKm);
        double ratingScore =
            rating.HasValue ? rating.Value / 5.0 : 0;
        return 
            0.6 * relevanceScore +
            0.3 * distanceScore +
            0.1 * ratingScore;
    }
    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}