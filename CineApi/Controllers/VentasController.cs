using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;

    public VentasController(IVentaService ventaService)
    {
        _ventaService = ventaService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetVentas()
    {
        return Ok(await _ventaService.GetVentasAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("codigo/{codigo}")]
    public async Task<IActionResult> GetVentaByCodigo(string codigo)
    {
        var venta = await _ventaService.GetVentaByCodigoAsync(codigo);

        if (venta == null)
            return NotFound("Venta no encontrada");

        return Ok(venta);
    }

    [Authorize]
    [HttpPost("confirmar")]
    public async Task<IActionResult> ConfirmarCompra(ConfirmarCompraDTO dto)
    {
        try
        {
            var result = await _ventaService.ConfirmarCompraAsync(dto);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                error = ex.Message
            });
        }
    }
}