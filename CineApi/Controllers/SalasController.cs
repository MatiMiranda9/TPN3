using CineApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalasController : ControllerBase
{
    private readonly ISalaService _salaService;

    public SalasController(ISalaService salaService)
    {
        _salaService = salaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSalas()
    {
        return Ok(await _salaService.GetAllAsync());
    }
}