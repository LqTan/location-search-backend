using Reviews.Domain.Entities;

namespace Reviews.Application.Abstractions;

public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<Review?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Review>> GetByPlaceIdAsync(Guid placeId);
    Task<double?> GetAverageRatingByPlaceIdAsync(Guid placeId);
}