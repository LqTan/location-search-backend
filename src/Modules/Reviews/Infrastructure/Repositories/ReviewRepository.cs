using Microsoft.EntityFrameworkCore;
using Reviews.Application.Abstractions;
using Reviews.Domain.Entities;
using Reviews.Infrastructure.Persistence;

namespace Reviews.Infrastructure.Repositories;

public sealed class ReviewRepository : IReviewRepository
{
    private readonly ReviewsDbContext _dbContext;

    public ReviewRepository(ReviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(Review review)
    {
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Reviews
            .FirstOrDefaultAsync(review => review.Id == id);
    }
    public async Task<IReadOnlyList<Review>> GetByPlaceIdAsync(Guid placeId)
    {
        return await _dbContext.Reviews
            .Where(review => review.PlaceId == placeId)
            .OrderByDescending(review => review.CreatedAt)
            .ToListAsync();
    }
    public async Task<double?> GetAverageRatingByPlaceIdAsync(Guid placeId)
    {
        return await _dbContext.Reviews
            .Where(review => review.PlaceId == placeId)
            .Select(review => (double?)review.Rating)
            .AverageAsync();
    }
}