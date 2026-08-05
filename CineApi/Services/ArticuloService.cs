using CineApi.Data;
using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class ArticuloService : IArticuloService
    {
        private readonly CineDBContext _context;

        public ArticuloService(CineDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetArticulos()
        {
            return await _context.Articulos
                .Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Precio,
                    a.Descripcion,
                    a.Stock,
                    a.Categoria,
                    a.ImagePath,
                    a.IsActive
                })
                .ToListAsync();
        }

        public async Task<object?> GetArticulo(int id)
        {
            return await _context.Articulos
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Precio,
                    a.Descripcion,
                    a.Stock,
                    a.Categoria,
                    a.ImagePath,
                    a.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Articulo> CrearArticulo(ArticuloDTO dto)
        {
            var articulo = new Articulo
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Descripcion = dto.Descripcion,
                Stock = dto.Stock,
                Categoria = dto.Categoria,
                ImagePath = dto.ImagePath,
                IsActive = true
            };

            _context.Articulos.Add(articulo);

            await _context.SaveChangesAsync();

            return articulo;
        }

        public async Task<Articulo?> EditarArticulo(int id, ArticuloDTO dto)
        {
            var articulo = await _context.Articulos.FindAsync(id);

            if (articulo == null)
                return null;

            articulo.Nombre = dto.Nombre;
            articulo.Precio = dto.Precio;
            articulo.Descripcion = dto.Descripcion;
            articulo.Stock = dto.Stock;
            articulo.Categoria = dto.Categoria;
            articulo.ImagePath = dto.ImagePath;

            await _context.SaveChangesAsync();

            return articulo;
        }

        public async Task<bool> EliminarArticulo(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);

            if (articulo == null)
                return false;

            articulo.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivarArticulo(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);

            if (articulo == null)
                return false;

            articulo.IsActive = true;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
