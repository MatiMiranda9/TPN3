using CineApi.Data;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimesController : ControllerBase
    {
        private readonly CineDBContext _context;

        public ShowtimesController(CineDBContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowtime(int id)
        {
            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Sala)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (showtime == null)
                return NotFound("Función no encontrada");

            var dto = new ShowtimeDTO
            {
                Id = showtime.Id,
                Time = showtime.Time,
                Price = showtime.Price,
                SalaId = showtime.SalaId,
                SalaNombre = showtime.Sala.Nombre,
                MovieId = showtime.MovieId,
                MovieNombre = showtime.Movie.Title,
                ImagePath = showtime.Movie.ImagePath,
                IsActive = showtime.IsActive
            };

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("{id}/asientos")]
        public async Task<IActionResult> GetAsientos(int id)
        {
            var showtime = await _context.Showtimes
                .Include(s => s.Sala)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (showtime == null)
                return NotFound("Función no encontrada");

            var ocupados = await _context.Tickets
                .Where(t => t.ShowtimeId == id)
                .Select(t => t.Asiento)
                .ToListAsync();

            var result = new AsientosDisponibilidadDTO
            {
                Capacidad = showtime.Sala.Capacidad,
                Ocupados = ocupados
            };

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("disable/{id}")]
        public async Task<IActionResult> DisableShowtime(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);

            if (showtime == null)
            {
                return NotFound();
            }

            showtime.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
