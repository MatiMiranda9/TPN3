using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoBlazorMovil.Shared.DTOs;
using System.Net.Http.Json;

namespace DemoBlazorMovil.Services
{

    public class VentaService
    {
        private readonly HttpClient _http;

        public VentaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<(bool Success, CompraResponseDTO? Data, string Message)> ConfirmarCompra(ConfirmarCompraDTO dto)
        {
            var response = await _http.PostAsJsonAsync("ventas/confirmar", dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CompraResponseDTO>();
                return (true, result, result?.mensaje ?? "Compra realizada");
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponseDTO>();
                return (false, null, error?.error ?? "Error al confirmar compra");
            }
        }
    }
}
