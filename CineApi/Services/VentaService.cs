using CineApi.Data;
using CineApi.Services.Interfaces;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services;

public class VentaService : IVentaService
{
    private readonly CineDBContext _context;

    public VentaService(CineDBContext context)
    {
        _context = context;
    }

    public async Task<List<VentaDTO>> GetVentasAsync()
    {
        return await _context.Ventas
            .OrderByDescending(v => v.Fecha)
            .Take(20)
            .Select(v => new VentaDTO
            {
                Id = v.Id,
                Codigo = v.Codigo,
                Fecha = v.Fecha,
                Total = v.Total
            })
            .ToListAsync();
    }

    public async Task<VentaDTO?> GetVentaByCodigoAsync(string codigo)
    {
        var venta = await _context.Ventas
            .Include(v => v.DetallesVenta)
                .ThenInclude(d => d.Articulo)

            .Include(v => v.DetallesVenta)
                .ThenInclude(d => d.Ticket)
                    .ThenInclude(t => t.Showtime)
                        .ThenInclude(s => s.Movie)

            .Include(v => v.DetallesVenta)
                .ThenInclude(d => d.Ticket)
                    .ThenInclude(t => t.Showtime)
                        .ThenInclude(s => s.Sala)

            .AsSplitQuery()
            .FirstOrDefaultAsync(v => v.Codigo == codigo);

        if (venta == null)
            return null;

        var dto = new VentaDTO
        {
            Id = venta.Id,
            Codigo = venta.Codigo,
            Fecha = venta.Fecha,
            Total = venta.Total
        };

        foreach (var d in venta.DetallesVenta)
        {
            if (d.Articulo != null)
            {
                dto.Articulos.Add(new VentaArticuloDTO
                {
                    Nombre = d.Articulo.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                });
            }

            if (d.Ticket != null)
            {
                dto.Tickets.Add(new VentaTicketDTO
                {
                    MovieNombre = d.Ticket.Showtime.Movie.Title,
                    Fecha = d.Ticket.Showtime.Time,
                    Sala = d.Ticket.Showtime.Sala.Nombre,
                    Asiento = d.Ticket.Asiento,
                    Precio = d.PrecioUnitario
                });
            }
        }

        return dto;
    }

    public async Task<CompraResponseDTO> ConfirmarCompraAsync(ConfirmarCompraDTO dto)
    {
        if (dto == null)
            throw new Exception("Datos inválidos");

        if ((dto.Articulos == null || !dto.Articulos.Any()) &&
            (dto.Tickets == null || !dto.Tickets.Any()))
        {
            throw new Exception("La compra está vacía");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var user = await _context.Users.FindAsync(dto.UserId);

            if (user == null)
                throw new Exception("Usuario inválido");

            decimal total = 0;

            var venta = new Venta
            {
                Fecha = DateTime.Now,
                UserId = dto.UserId,
                Codigo = $"VENTA-{DateTime.Now:yyyyMMddHHmmss}",
                Total = 0
            };

            _context.Ventas.Add(venta);

            // ====================
            // ARTÍCULOS
            // ====================

            foreach (var articuloDto in dto.Articulos ?? [])
            {
                var articulo = await _context.Articulos
                    .FirstOrDefaultAsync(a => a.Id == articuloDto.ArticuloId);

                if (articulo == null)
                    throw new Exception("Artículo no encontrado");

                if (articulo.Stock < articuloDto.Cantidad)
                    throw new Exception($"Stock insuficiente para {articulo.Nombre}");

                articulo.Stock -= articuloDto.Cantidad;

                total += articulo.Precio * articuloDto.Cantidad;

                venta.DetallesVenta.Add(new DetalleVenta
                {
                    ArticuloId = articulo.Id,
                    Cantidad = articuloDto.Cantidad,
                    PrecioUnitario = articulo.Precio
                });
            }

            // ====================
            // TICKETS
            // ====================

            foreach (var ticketDto in dto.Tickets ?? [])
            {
                var showtime = await _context.Showtimes
                    .Include(s => s.Sala)
                    .FirstOrDefaultAsync(s => s.Id == ticketDto.ShowtimeId);

                if (showtime == null)
                    throw new Exception("Función no encontrada");

                if (ticketDto.Asiento < 1 ||
                    ticketDto.Asiento > showtime.Sala.Capacidad)
                {
                    throw new Exception("Asiento inválido");
                }

                bool asientoOcupado = await _context.Tickets
                    .AnyAsync(t =>
                        t.ShowtimeId == ticketDto.ShowtimeId &&
                        t.Asiento == ticketDto.Asiento);

                if (asientoOcupado)
                    throw new Exception($"El asiento {ticketDto.Asiento} ya está ocupado");

                var ticket = new Ticket
                {
                    ShowtimeId = ticketDto.ShowtimeId,
                    Asiento = ticketDto.Asiento
                };

                _context.Tickets.Add(ticket);

                total += showtime.Price;

                venta.DetallesVenta.Add(new DetalleVenta
                {
                    Ticket = ticket,
                    Cantidad = 1,
                    PrecioUnitario = showtime.Price
                });
            }

            venta.Total = total;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new CompraResponseDTO
            {
                mensaje = "Compra realizada con éxito",
                ventaId = venta.Id,
                codigo = venta.Codigo,
                total = venta.Total
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}