using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;

namespace CineApi.Services.Interfaces
{
    public interface IArticuloService
    {
        Task<IEnumerable<object>> GetArticulos();

        Task<object?> GetArticulo(int id);

        Task<Articulo> CrearArticulo(ArticuloDTO dto);

        Task<Articulo?> EditarArticulo(int id, ArticuloDTO dto);

        Task<bool> EliminarArticulo(int id);

        Task<bool> ActivarArticulo(int id);
    }
}
