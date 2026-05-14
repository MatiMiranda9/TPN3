using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly IArticuloService _articuloService;

        public ArticulosController(IArticuloService articuloService)
        {
            _articuloService = articuloService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticulos()
        {
            var articulos = await _articuloService.GetArticulos();

            return Ok(articulos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticulo(int id)
        {
            var articulo = await _articuloService.GetArticulo(id);

            if (articulo == null)
                return NotFound();

            return Ok(articulo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CrearArticulo(ArticuloDTO dto)
        {
            var articulo = await _articuloService.CrearArticulo(dto);

            return Ok(articulo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarArticulo(int id, ArticuloDTO dto)
        {
            var articulo = await _articuloService.EditarArticulo(id, dto);

            if (articulo == null)
                return NotFound();

            return Ok(articulo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarArticulo(int id)
        {
            var ok = await _articuloService.ActivarArticulo(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarArticulo(int id)
        {
            var ok = await _articuloService.EliminarArticulo(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}