using CineApi.Data;
using DemoBlazorMovil.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly CineDbContext _context;

    public MoviesController(CineDbContext context)
    {
        _context = context;
    }

    // ================== PELÍCULAS ==================

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        return await _context.Movies
            .Include(m => m.Showtimes)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Showtimes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();
        return movie;
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie(Movie movie)
    {
        // aseguramos que cada showtime tenga MovieId
        foreach (var st in movie.Showtimes)
            st.MovieId = movie.Id;

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, Movie movie)
    {
        if (id != movie.Id) return BadRequest();

        var existingMovie = await _context.Movies
            .Include(m => m.Showtimes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMovie == null) return NotFound();

        // Actualizamos propiedades principales
        existingMovie.Title = movie.Title;
        existingMovie.Genre = movie.Genre;
        existingMovie.Year = movie.Year;
        existingMovie.ImagePath = movie.ImagePath;

        // Reemplazamos Showtimes
        existingMovie.Showtimes.Clear();
        foreach (var st in movie.Showtimes)
        {
            st.MovieId = existingMovie.Id;
            existingMovie.Showtimes.Add(st);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Showtimes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        _context.Showtimes.RemoveRange(movie.Showtimes);
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ================== SHOWTIMES (anidados) ==================

    [HttpGet("{movieId}/showtimes")]
    public async Task<ActionResult<IEnumerable<Showtime>>> GetShowtimesForMovie(int movieId)
    {
        var movie = await _context.Movies
            .Include(m => m.Showtimes)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null) return NotFound("Película no encontrada");

        return movie.Showtimes;
    }

    [HttpPost("{movieId}/showtimes")]
    public async Task<ActionResult<Showtime>> AddShowtime(int movieId, Showtime showtime)
    {
        var movie = await _context.Movies.FindAsync(movieId);
        if (movie == null) return NotFound("Película no encontrada");

        showtime.MovieId = movieId;
        _context.Showtimes.Add(showtime);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShowtimesForMovie), new { movieId }, showtime);
    }

    [HttpPut("{movieId}/showtimes/{showtimeId}")]
    public async Task<IActionResult> UpdateShowtime(int movieId, int showtimeId, Showtime showtime)
    {
        if (showtimeId != showtime.Id) return BadRequest();

        var existing = await _context.Showtimes
            .FirstOrDefaultAsync(s => s.Id == showtimeId && s.MovieId == movieId);

        if (existing == null) return NotFound();

        existing.Time = showtime.Time;
        existing.Room = showtime.Room;
        existing.Price = showtime.Price;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{movieId}/showtimes/{showtimeId}")]
    public async Task<IActionResult> DeleteShowtime(int movieId, int showtimeId)
    {
        var showtime = await _context.Showtimes
            .FirstOrDefaultAsync(s => s.Id == showtimeId && s.MovieId == movieId);

        if (showtime == null) return NotFound();

        _context.Showtimes.Remove(showtime);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
