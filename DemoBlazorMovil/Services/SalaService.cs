using DemoBlazorMovil.Shared.DTOs;
using System.Net.Http.Json;

namespace DemoBlazorMovil.Services;

public class SalaService
{
    private readonly HttpClient _http;

    public SalaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SalaDTO>> GetAll() 
        =>await _http.GetFromJsonAsync<List<SalaDTO>>("Salas") ?? new();
    
}
