using DemoBlazorMovil.Shared.DTOs;

namespace CineApi.Services.Interfaces;

public interface IVentaService
{
    Task<List<VentaDTO>> GetVentasAsync();

    Task<VentaDTO?> GetVentaByCodigoAsync(string codigo);

    Task<CompraResponseDTO> ConfirmarCompraAsync(ConfirmarCompraDTO dto);
}