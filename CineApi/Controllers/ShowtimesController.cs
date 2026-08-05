using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimesController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;

        public ShowtimesController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetByMovie(int movieId)
        {
            return Ok(await _showtimeService.GetByMovieAsync(movieId));
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowtime(int id)
        {
            var showtime = await _showtimeService.GetByIdAsync(id);

            if (showtime == null)
                return NotFound();

            return Ok(showtime);
        }

        [Authorize]
        [HttpGet("{id}/asientos")]
        public async Task<IActionResult> GetAsientos(int id)
        {
            var result = await _showtimeService
                .GetDisponibilidadAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ShowtimeDTO dto)
        {
            var created = await _showtimeService.CreateAsync(dto);

            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ShowtimeDTO dto)
        {
            var ok = await _showtimeService.UpdateAsync(id, dto);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("disable/{id}")]
        public async Task<IActionResult> Disable(int id)
        {
            var ok = await _showtimeService.DisableAsync(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}