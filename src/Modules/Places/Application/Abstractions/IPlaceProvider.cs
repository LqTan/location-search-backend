using Places.Domain.Entities;

namespace Places.Application.Abstractions;

public interface IPlaceProvider
{
    Task<IReadOnlyList<Place>> SearchAsync(
        string query,
        double latitude,
        double longitude,
        double radiusKm,
        CancellationToken cancellationToken = default
    );
}