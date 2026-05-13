using DemoBlazorMovil.Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Json;


namespace DemoBlazorMovil.Services;

public class MovieService
{
    private readonly HttpClient _http;

    public MovieService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<MovieDTO>> GetAll()
        => await _http.GetFromJsonAsync<List<MovieDTO>>("movies") ?? new();

    public async Task<MovieDTO?> GetById(int id)
        => await _http.GetFromJsonAsync<MovieDTO>($"movies/{id}");

    public async Task<MovieDTO?> Add(MovieDTO movie)
    {
        var response = await _http.PostAsJsonAsync("movies", movie);
        return await response.Content.ReadFromJsonAsync<MovieDTO>();
    }

    public async Task<bool> Update(MovieDTO movie)
    {
        var response = await _http.PutAsJsonAsync($"movies/{movie.Id}", movie);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(int id)
    {
        var response = await _http.DeleteAsync($"movies/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Activar(int id)
    {
        var response = await _http.PutAsync($"movies/activar/{id}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<ShowtimeDTO>> GetShowtimes(int movieId)
        => await _http.GetFromJsonAsync<List<ShowtimeDTO>>($"movies/{movieId}/showtimes") ?? new();

    public async Task<ShowtimeDTO?> AddShowtime(int movieId, ShowtimeDTO showtime)
    {
        var response = await _http.PostAsJsonAsync($"movies/{movieId}/showtimes", showtime);
        return await response.Content.ReadFromJsonAsync<ShowtimeDTO>();
    }

    public async Task<bool> UpdateShowtime(int movieId, ShowtimeDTO showtime)
    {
        var response = await _http.PutAsJsonAsync($"movies/{movieId}/showtimes/{showtime.Id}", showtime);
        return response.IsSuccessStatusCode;
    }
    public async Task DisableShowtime(int id)
    {
        await _http.PutAsync($"showtimes/disable/{id}", null);
    }
    public async Task<bool> DeleteShowtime(int movieId, int showtimeId)
    {
        var response = await _http.DeleteAsync($"movies/{movieId}/showtimes/{showtimeId}");
        return response.IsSuccessStatusCode;
    }
}
