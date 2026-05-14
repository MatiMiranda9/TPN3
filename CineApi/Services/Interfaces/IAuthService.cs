using DemoBlazorMovil.Shared.DTOs;

namespace CineApi.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDTO dto);

    Task<LoginResponseDTO?> LoginAsync(UserLoginDto dto);
}