using Microsoft.AspNetCore.Mvc;
using Reviews.Application.Reviews.Commands.CreateReview;
using Reviews.Application.Reviews.Queries.GetAverageRatingByPlace;
using Reviews.Application.Reviews.Queries.GetReviewsByPlace;

namespace Reviews.Presentation;

[ApiController]
[Route("api/reviews")]
public sealed class ReviewsController : ControllerBase
{
    private readonly CreateReviewHandler _createReviewHandler;
    private readonly GetReviewsByPlaceHandler _getReviewsByPlaceHandler;
    private readonly GetAverageRatingByPlaceHandler _getAverageRatingByPlaceHandler;

    public ReviewsController(
        CreateReviewHandler createReviewHandler,
        GetReviewsByPlaceHandler getReviewsByPlaceHandler,
        GetAverageRatingByPlaceHandler getAverageRatingByPlaceHandler
    )
    {
        _createReviewHandler = createReviewHandler;
        _getReviewsByPlaceHandler = getReviewsByPlaceHandler;
        _getAverageRatingByPlaceHandler = getAverageRatingByPlaceHandler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateReviewResult>> Create(
        CreateReviewCommand command
    )
    {
        var result = await _createReviewHandler.HandleAsync(command);
        return Ok(result);
    }

    [HttpGet("place/{placeId:guid}")]
    public async Task<ActionResult<IReadOnlyList<GetReviewsByPlaceResult>>> GetByPlace(
        Guid placeId
    )
    {
        var query = new GetReviewsByPlaceQuery(placeId);
        var result = await _getReviewsByPlaceHandler.HandleAsync(query);
        return Ok(result);
    }

    [HttpGet("place/{placeId:guid}/average-rating")]
    public async Task<ActionResult<GetAverageRatingByPlaceResult>> GetAverageRating(
        Guid placeId
    )
    {
        var query = new GetAverageRatingByPlaceQuery(placeId);
        var result = await _getAverageRatingByPlaceHandler.HandleAsync(query);
        return Ok(result);
    }
}