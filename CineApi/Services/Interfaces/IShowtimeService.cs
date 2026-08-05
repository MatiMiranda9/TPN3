using DemoBlazorMovil.Shared.DTOs;

namespace CineApi.Services.Interfaces
{
    public interface IShowtimeService
    {
        Task<List<ShowtimeDTO>> GetByMovieAsync(int movieId);

        Task<ShowtimeDTO?> GetByIdAsync(int id);

        Task<AsientosDisponibilidadDTO?> GetDisponibilidadAsync(int id);

        Task<ShowtimeDTO> CreateAsync(ShowtimeDTO dto);

        Task<bool> UpdateAsync(int id, ShowtimeDTO dto);

        Task<bool> DisableAsync(int id);
    }
}