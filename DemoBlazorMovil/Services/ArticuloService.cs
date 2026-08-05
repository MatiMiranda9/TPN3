using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using DemoBlazorMovil.Shared.DTOs;

namespace DemoBlazorMovil.Services
{
    

    public class ArticuloService
    {
        private readonly HttpClient _http;

        public ArticuloService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ArticuloDTO>> GetAll()
        {
            return await _http.GetFromJsonAsync<List<ArticuloDTO>>("articulos")
                   ?? new List<ArticuloDTO>();
        }

        public async Task<ArticuloDTO?> Add(ArticuloDTO articulo)
        {
            var response = await _http.PostAsJsonAsync("articulos", articulo);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ArticuloDTO>();
        }

        public async Task<bool> Update(ArticuloDTO articulo)
        {
            var response = await _http.PutAsJsonAsync($"articulos/{articulo.Id}", articulo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _http.DeleteAsync($"articulos/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Activar(int id)
        {
            var response = await _http.PutAsync($"articulos/activar/{id}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
