using Reviews.Application.Abstractions;

namespace Reviews.Application.Reviews.Queries.GetAverageRatingByPlace;

public sealed class GetAverageRatingByPlaceHandler
{
    private readonly IReviewRepository _reviewRepository;
    public GetAverageRatingByPlaceHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    public async Task<GetAverageRatingByPlaceResult> HandleAsync(
        GetAverageRatingByPlaceQuery query
    )
    {
        var averageRating = await _reviewRepository.GetAverageRatingByPlaceIdAsync(query.PlaceId);
        return new GetAverageRatingByPlaceResult(
            query.PlaceId,
            averageRating
        );
    }
}