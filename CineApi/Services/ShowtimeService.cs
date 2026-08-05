using CineApi.Data;
using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly CineDBContext _context;

        public ShowtimeService(CineDBContext context)
        {
            _context = context;
        }

        public async Task<List<ShowtimeDTO>> GetByMovieAsync(int movieId)
        {
            var showtimes = await _context.Showtimes
                .Include(s => s.Sala)
                .Where(s => s.MovieId == movieId)
                .ToListAsync();

            return showtimes.Select(MapShowtime).ToList();
        }

        public async Task<ShowtimeDTO?> GetByIdAsync(int id)
        {
            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Sala)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (showtime == null)
                return null;

            return new ShowtimeDTO
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
        }

        public async Task<AsientosDisponibilidadDTO?> GetDisponibilidadAsync(int id)
        {
            var showtime = await _context.Showtimes
                .Include(s => s.Sala)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (showtime == null)
                return null;

            var ocupados = await _context.Tickets
                .Where(t => t.ShowtimeId == id)
                .Select(t => t.Asiento)
                .ToListAsync();

            return new AsientosDisponibilidadDTO
            {
                Capacidad = showtime.Sala.Capacidad,
                Ocupados = ocupados
            };
        }

        public async Task<ShowtimeDTO> CreateAsync(ShowtimeDTO dto)
        {
            var showtime = new Showtime
            {
                MovieId = dto.MovieId,
                Time = dto.Time,
                SalaId = dto.SalaId,
                Price = dto.Price,
                IsActive = true
            };

            _context.Showtimes.Add(showtime);

            await _context.SaveChangesAsync();

            dto.Id = showtime.Id;

            return dto;
        }

        public async Task<bool> UpdateAsync(int id, ShowtimeDTO dto)
        {
            var showtime = await _context.Showtimes.FindAsync(id);

            if (showtime == null)
                return false;

            showtime.Time = dto.Time;
            showtime.SalaId = dto.SalaId;
            showtime.Price = dto.Price;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DisableAsync(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);

            if (showtime == null)
                return false;

            showtime.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

        private static ShowtimeDTO MapShowtime(Showtime s)
        {
            return new ShowtimeDTO
            {
                Id = s.Id,
                Time = s.Time,
                Price = s.Price,
                SalaId = s.SalaId,
                SalaNombre = s.Sala.Nombre,
                MovieId = s.MovieId,
                IsActive = s.IsActive
            };
        }
    }
}