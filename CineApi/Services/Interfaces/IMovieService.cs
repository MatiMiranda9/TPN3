using DemoBlazorMovil.Shared.DTOs;

namespace CineApi.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieDTO>> GetAllAsync();

        Task<MovieDTO?> GetByIdAsync(int id);

        Task<MovieDTO> CreateAsync(MovieDTO dto);

        Task<bool> UpdateAsync(int id, MovieDTO dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ActivateAsync(int id);
    }
}