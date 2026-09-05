using Reviews.Application.Abstractions;
using Reviews.Domain.Entities;

namespace Reviews.Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewHandler
{
    private readonly IReviewRepository _reviewRepository;

    public CreateReviewHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    public async Task<CreateReviewResult> HandleAsync(
        CreateReviewCommand command
    )
    {
        var review = new Review(
            command.PlaceId,
            command.UserId,
            command.Rating,
            command.Comment
        );
        await _reviewRepository.AddAsync(review);
        return new CreateReviewResult(
            review.Id,
            review.PlaceId,
            review.UserId,
            review.Rating,
            review.Comment,
            review.CreatedAt
        );
    }
}