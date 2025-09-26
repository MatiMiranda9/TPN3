using System.Net.Http.Json;
using DemoBlazorMovil.Shared.Models;

namespace DemoBlazorMovil.Services;

public class MovieService
{
    private readonly HttpClient _http;

    public MovieService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Movie>> GetAll()
        => await _http.GetFromJsonAsync<List<Movie>>("movies") ?? new();

    public async Task<Movie?> GetById(int id)
        => await _http.GetFromJsonAsync<Movie>($"movies/{id}");

    public async Task<Movie?> Add(Movie movie)
    {
        var response = await _http.PostAsJsonAsync("movies", movie);
        return await response.Content.ReadFromJsonAsync<Movie>();
    }

    public async Task<bool> Update(Movie movie)
    {
        var response = await _http.PutAsJsonAsync($"movies/{movie.Id}", movie);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Delete(int id)
    {
        var response = await _http.DeleteAsync($"movies/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Showtime>> GetShowtimes(int movieId)
        => await _http.GetFromJsonAsync<List<Showtime>>($"movies/{movieId}/showtimes") ?? new();

    public async Task<Showtime?> AddShowtime(int movieId, Showtime showtime)
    {
        var response = await _http.PostAsJsonAsync($"movies/{movieId}/showtimes", showtime);
        return await response.Content.ReadFromJsonAsync<Showtime>();
    }

    public async Task<bool> UpdateShowtime(int movieId, Showtime showtime)
    {
        var response = await _http.PutAsJsonAsync($"movies/{movieId}/showtimes/{showtime.Id}", showtime);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteShowtime(int movieId, int showtimeId)
    {
        var response = await _http.DeleteAsync($"movies/{movieId}/showtimes/{showtimeId}");
        return response.IsSuccessStatusCode;
    }
}
