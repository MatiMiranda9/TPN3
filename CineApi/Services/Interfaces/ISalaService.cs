using DemoBlazorMovil.Shared.DTOs;

namespace CineApi.Services.Interfaces;

public interface ISalaService
{
    Task<List<SalaDTO>> GetAllAsync();
}