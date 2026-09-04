using Microsoft.AspNetCore.Mvc;
using Search.Application.Search.Queries.SearchPlaces;
using SearchPlacesHandler = Search.Application.Search.Queries.SearchPlaces.SearchPlacesHandler;

namespace Search.Presentation.Controllers;

[ApiController]
[Route("api/search")]
public sealed class SearchController : ControllerBase
{
    private readonly SearchPlacesHandler _handler;
    public SearchController(SearchPlacesHandler handler)
    {
        _handler = handler;
    }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SearchResult>>> Search(
        [FromQuery] string query,
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm = 5,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _handler.HandleAsync(
            new SearchPlacesQuery(
                query,
                latitude,
                longitude,
                radiusKm
            ),
            cancellationToken
        );
        return Ok(result);
    }
}