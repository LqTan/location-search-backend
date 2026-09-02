using Microsoft.AspNetCore.Mvc;
using Places.Application.Places.Queries.GetPlaceById;
using Places.Application.Places.Queries.SearchPlaces;

namespace Places.Presentation.Controllers;

[ApiController]
[Route("api/places")]
public sealed class PlacesController : ControllerBase
{
    private readonly SearchPlacesHandler _searchPlacesHandler;
    private readonly GetPlaceByIdHandler _getPlacesByIdHandler;

    public PlacesController(
        SearchPlacesHandler searchPlacesHandler,
        GetPlaceByIdHandler getPlaceByIdHandler
    )
    {
        _searchPlacesHandler = searchPlacesHandler;
        _getPlacesByIdHandler = getPlaceByIdHandler;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string query,
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm,
        CancellationToken cancellationToken
    )
    {
        var searchQuery = new SearchPlacesQuery(
            query,
            latitude,
            longitude,
            radiusKm
        );
        var places = await _searchPlacesHandler.HandleAsync(
            searchQuery,
            cancellationToken
        );
        return Ok(places);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetPlaceByIdQuery(id);
        var place = await _getPlacesByIdHandler.HandleAsync(
            query,
            cancellationToken
        );
        return place is null ? NotFound() : Ok(place);
    }
}