using Reviews.Application.Abstractions;

namespace Reviews.Application.Reviews.Queries.GetReviewsByPlace;

public sealed class GetReviewsByPlaceHandler
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsByPlaceHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    public async Task<IReadOnlyList<GetReviewsByPlaceResult>> HandleAsync(
        GetReviewsByPlaceQuery query
    )
    {
        var reviews = await _reviewRepository.GetByPlaceIdAsync(query.PlaceId);
        return reviews
            .Select(review => new GetReviewsByPlaceResult(
                review.Id,
                review.PlaceId,
                review.UserId,
                review.Rating,
                review.Comment,
                review.CreatedAt,
                review.UpdatedAt            
            ))
            .ToList();
    }
}