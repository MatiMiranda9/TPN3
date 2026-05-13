using CineApi.Data;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly CineDBContext _context;

    public MoviesController(CineDBContext context)
    {
        _context = context;
    }

    // ================== PELÍCULAS ==================

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
    {
        var movies = await _context.Movies
            .Include(m => m.Showtimes)
            .ThenInclude(s => s.Sala)
            .ToListAsync();

        var result = movies.Select(m => new MovieDTO
        {
            Id = m.Id,
            Title = m.Title,
            Genre = m.Genre,
            Year = m.Year,
            ImagePath = m.ImagePath,
            IsActive = m.IsActive,
            Showtimes = m.Showtimes.Select(s => new ShowtimeDTO
            {
                Id = s.Id,
                Time = s.Time,
                Price = s.Price,
                SalaId = s.SalaId,
                SalaNombre = s.Sala.Nombre,
                IsActive = s.IsActive,
                MovieId = s.MovieId
            }).ToList()
        }).ToList();

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDTO>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Showtimes)
            .ThenInclude(s => s.Sala)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        var result = new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            Year = movie.Year,
            ImagePath = movie.ImagePath,
            IsActive = movie.IsActive,
            Showtimes = movie.Showtimes.Select(s => new ShowtimeDTO
            {
                Id = s.Id,
                Time = s.Time,
                Price = s.Price,
                SalaId = s.SalaId,
                SalaNombre = s.Sala.Nombre,
                IsActive = s.IsActive,
                MovieId = s.MovieId
            }).ToList()
        };

        return result;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO movieDto)
    {
        var movie = new Movie
        {
            Title = movieDto.Title,
            Genre = movieDto.Genre,
            Year = movieDto.Year,
            ImagePath = movieDto.ImagePath,
            Showtimes = movieDto.Showtimes.Select(s => new Showtime
            {
                Time = s.Time,
                SalaId = s.SalaId,
                IsActive = s.IsActive,
                Price = s.Price
            }).ToList()
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        movieDto.Id = movie.Id;

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, MovieDTO movieDto)
    {
        if (id != movieDto.Id) return BadRequest();

        var existingMovie = await _context.Movies
            .Include(m => m.Showtimes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMovie == null) return NotFound();

        existingMovie.Title = movieDto.Title;
        existingMovie.Genre = movieDto.Genre;
        existingMovie.Year = movieDto.Year;
        existingMovie.ImagePath = movieDto.ImagePath;

        foreach (var showDto in movieDto.Showtimes)
        {
            var existingShow = existingMovie.Showtimes
                .FirstOrDefault(s => s.Id == showDto.Id);

            if (existingShow != null)
            {
                // actualizar existente
                existingShow.Time = showDto.Time;
                existingShow.SalaId = showDto.SalaId;
                existingShow.Price = showDto.Price;
                existingShow.IsActive = showDto.IsActive;
            }
            else
            {
                // nuevo showtime
                existingMovie.Showtimes.Add(new Showtime
                {
                    Time = showDto.Time,
                    SalaId = showDto.SalaId,
                    Price = showDto.Price,
                    IsActive = true,
                    MovieId = id
                });
            }
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> ActivarMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null) return NotFound();

        movie.IsActive = true;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return NotFound();

        movie.IsActive = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ================== SHOWTIMES (anidados) ==================

    [HttpGet("{movieId}/showtimes")]
    public async Task<ActionResult<IEnumerable<ShowtimeDTO>>> GetShowtimesForMovie(int movieId)
    {
        var showtimes = await _context.Showtimes
            .Include(s => s.Sala)
            .Where(s => s.MovieId == movieId)
            .ToListAsync();

        var result = showtimes.Select(s => new ShowtimeDTO
        {
            Id = s.Id,
            Time = s.Time,
            Price = s.Price,
            SalaId = s.SalaId,
            SalaNombre = s.Sala.Nombre,
            IsActive = s.IsActive,
            MovieId = s.MovieId
        }).ToList();

        return result;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{movieId}/showtimes")]
    public async Task<ActionResult<ShowtimeDTO>> AddShowtime(int movieId, ShowtimeDTO dto)
    {
        var movie = await _context.Movies.FindAsync(movieId);
        if (movie == null) return NotFound();

        var showtime = new Showtime
        {
            MovieId = movieId,
            Time = dto.Time,
            SalaId = dto.SalaId,
            IsActive = true,
            Price = dto.Price
        };

        _context.Showtimes.Add(showtime);
        await _context.SaveChangesAsync();

        dto.Id = showtime.Id;
        return CreatedAtAction(nameof(GetShowtimesForMovie), new { movieId }, dto);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{movieId}/showtimes/{showtimeId}")]
    public async Task<IActionResult> UpdateShowtime(int movieId, int showtimeId, ShowtimeDTO dto)
    {
        if (showtimeId != dto.Id)
            return BadRequest();

        var existing = await _context.Showtimes
            .FirstOrDefaultAsync(s => s.Id == showtimeId && s.MovieId == movieId);

        if (existing == null)
            return NotFound();

        existing.Time = dto.Time;
        existing.SalaId = dto.SalaId;
        existing.Price = dto.Price;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{movieId}/showtimes/{showtimeId}")]
    public async Task<IActionResult> DeleteShowtime(int movieId, int showtimeId)
    {
        var showtime = await _context.Showtimes
            .FirstOrDefaultAsync(s => s.Id == showtimeId && s.MovieId == movieId);

        if (showtime == null) return NotFound();

        showtime.IsActive = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    
}
