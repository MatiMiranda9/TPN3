using CineApi.Data;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly CineDBContext _context;

        public ArticulosController(CineDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticulos()
        {
            var articulos = await _context.Articulos
                .Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Precio,
                    a.Descripcion,
                    a.Stock,
                    a.Categoria,
                    a.ImagePath,
                    a.IsActive
                })
                .ToListAsync();

            return Ok(articulos);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticulo(int id)
        {
            var articulo = await _context.Articulos
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Precio,
                    a.Descripcion,
                    a.Stock,
                    a.Categoria,
                    a.ImagePath,
                    a.IsActive
                })
                .FirstOrDefaultAsync();

            if (articulo == null)
                return NotFound();

            return Ok(articulo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CrearArticulo(ArticuloDTO dto)
        {
            var articulo = new Articulo
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Descripcion = dto.Descripcion,
                Stock = dto.Stock,
                Categoria = dto.Categoria,
                ImagePath = dto.ImagePath,
                IsActive = true
            };

            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();

            return Ok(articulo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarArticulo(int id, ArticuloDTO dto)
        {
            var articulo = await _context.Articulos.FindAsync(id);

            if (articulo == null)
                return NotFound();

            articulo.Nombre = dto.Nombre;
            articulo.Precio = dto.Precio;
            articulo.Descripcion = dto.Descripcion;
            articulo.Stock = dto.Stock;
            articulo.Categoria = dto.Categoria;
            articulo.ImagePath = dto.ImagePath;

            await _context.SaveChangesAsync();

            return Ok(articulo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarArticulo(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);

            if (articulo == null) return NotFound();

            articulo.IsActive = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarArticulo(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);

            if (articulo == null)
                return NotFound();

            articulo.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
