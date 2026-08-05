using DemoBlazorMovil.Shared.DTOs;

namespace CineApi.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();

    Task<UserDto?> GetByIdAsync(int id);

    Task<UserDto> CreateAsync(UserCreateDto dto);

    Task<bool> UpdateAsync(int id, UserDto dto);

    Task<bool> DeleteAsync(int id);

    Task<bool> ActivateAsync(int id);
}