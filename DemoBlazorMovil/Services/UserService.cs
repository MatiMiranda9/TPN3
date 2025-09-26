using System.Net.Http.Json;
using DemoBlazorMovil.Shared.DTOs;

namespace DemoBlazorMovil.Services;

public class UserService
{
    private readonly HttpClient _http;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    // 📌 Obtener todos los usuarios
    public async Task<List<UserDto>> GetAll()
        => await _http.GetFromJsonAsync<List<UserDto>>("users") ?? new();

    // 📌 Obtener un usuario por Id
    public async Task<UserDto?> GetById(int id)
        => await _http.GetFromJsonAsync<UserDto>($"users/{id}");

    // 📌 Crear un usuario nuevo
    public async Task<UserDto?> Add(UserCreateDto user)
    {
        var response = await _http.PostAsJsonAsync("users", user);
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    // 📌 Actualizar un usuario existente
    public async Task<bool> Update(int id, UserCreateDto user)
    {
        var response = await _http.PutAsJsonAsync($"users/{id}", user);
        return response.IsSuccessStatusCode;
    }

    // 📌 Eliminar un usuario
    public async Task<bool> Delete(int id)
    {
        var response = await _http.DeleteAsync($"users/{id}");
        return response.IsSuccessStatusCode;
    }

    // 📌 Validar login contra la API
    public async Task<UserDto?> ValidateLogin(string email, string password)
    {
        var loginDto = new UserLoginDto { Email = email, Password = password };
        var response = await _http.PostAsJsonAsync("users/login", loginDto);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<UserDto>();

        return null;
    }
}
