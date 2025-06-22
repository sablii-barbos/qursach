using Microsoft.AspNetCore.Mvc;
using AutoriaBot.Services;
using AutoriaBot.Models;

namespace AutoriaBot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoApiController : ControllerBase
{
    private readonly AutoRiaService _autoRia;
    private readonly VinDecoderService _vinService;

    public AutoApiController(AutoRiaService autoRia, VinDecoderService vinService)
    {
        _autoRia = autoRia;
        _vinService = vinService;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFilters filters)
    {
        var results = await _autoRia.SearchAsync(filters);
        return Ok(results);
    }

    [HttpGet("vin/{vin}")]
    public async Task<IActionResult> Decode(string vin)
    {
        var result = await _vinService.DecodeVinAsync(vin);
        return Ok(result);
    }
}
