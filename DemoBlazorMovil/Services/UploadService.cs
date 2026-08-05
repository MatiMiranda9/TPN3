using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DemoBlazorMovil.Services;

public class UploadService
{
    private readonly HttpClient _http;

    public UploadService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string?> UploadImage(IBrowserFile file, string folder)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.Name);
            content.Add(new StringContent(folder), "folder");

            System.Diagnostics.Debug.WriteLine("Enviando request...");

            var response = await _http.PostAsync("upload/image", content);

            System.Diagnostics.Debug.WriteLine($"Status: {response.StatusCode}");

            var body = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine(body);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<UploadResponse>();

            System.Diagnostics.Debug.WriteLine(result?.Url);

            return result?.Url;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            throw;
        }
    }

    private class UploadResponse
    {
        public string Url { get; set; } = string.Empty;
    }
}