using CineApi.Data;
using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly CineDBContext _context;

        public MovieService(CineDBContext context)
        {
            _context = context;
        }

        public async Task<List<MovieDTO>> GetAllAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Showtimes)
                .ThenInclude(s => s.Sala)
                .ToListAsync();

            return movies.Select(MapMovie).ToList();
        }

        public async Task<MovieDTO?> GetByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Showtimes)
                .ThenInclude(s => s.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return null;

            return MapMovie(movie);
        }

        public async Task<MovieDTO> CreateAsync(MovieDTO dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Genre = dto.Genre,
                Year = dto.Year,
                ImagePath = dto.ImagePath,
                IsActive = true
            };

            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

            dto.Id = movie.Id;

            return dto;
        }

        public async Task<bool> UpdateAsync(int id, MovieDTO dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return false;

            movie.Title = dto.Title;
            movie.Genre = dto.Genre;
            movie.Year = dto.Year;
            movie.ImagePath = dto.ImagePath;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return false;

            movie.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return false;

            movie.IsActive = true;

            await _context.SaveChangesAsync();

            return true;
        }

        private static MovieDTO MapMovie(Movie m)
        {
            return new MovieDTO
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
                    MovieId = s.MovieId,
                    IsActive = s.IsActive
                }).ToList()
            };
        }
    }
}