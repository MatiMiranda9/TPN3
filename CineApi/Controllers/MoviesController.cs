using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            return Ok(await _movieService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO dto)
        {
            var created = await _movieService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetMovie),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO dto)
        {
            var ok = await _movieService.UpdateAsync(id, dto);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivateMovie(int id)
        {
            var ok = await _movieService.ActivateAsync(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var ok = await _movieService.DeleteAsync(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}