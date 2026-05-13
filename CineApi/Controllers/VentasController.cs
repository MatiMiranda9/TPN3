using CineApi.Data;
using DemoBlazorMovil.Shared.DTOs;
using DemoBlazorMovil.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace CineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly CineDBContext _context;

        public VentasController(CineDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetVentas()
        {
            var ventas = await _context.Ventas
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

            return Ok(ventas);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> GetVentaByCodigo(string codigo)
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
                return NotFound("Venta no encontrada");

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

            return Ok(dto);
        }

        [Authorize]
        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarCompra(ConfirmarCompraDTO dto)
        {
            if (dto == null)
                return BadRequest(new ErrorResponseDTO { error = "Datos inválidos" });

            if ((dto.Articulos == null || !dto.Articulos.Any()) &&
                (dto.Tickets == null || !dto.Tickets.Any()))
            {
                return BadRequest(new ErrorResponseDTO { error = "La compra está vacía" });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null)
                    return BadRequest(new ErrorResponseDTO { error = "Usuario inválido" });

                decimal total = 0;

                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    UserId = dto.UserId,
                    Codigo = $"VENTA-{DateTime.Now:yyyyMMddHHmmss}",
                    Total = total
                };

                _context.Ventas.Add(venta);

                // ============================
                // 🔹 ARTÍCULOS
                // ============================
                foreach (var articuloDto in dto.Articulos ?? new List<ArticuloCompraDTO>())
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
                        VentaId = venta.Id,
                        ArticuloId = articulo.Id,
                        Cantidad = articuloDto.Cantidad,
                        PrecioUnitario = articulo.Precio
                    });
                }

                // ============================
                // 🔹 TICKETS
                // ============================
                foreach (var ticketDto in dto.Tickets ?? new List<TicketCompraDTO>())
                {
                    var showtime = await _context.Showtimes
                        .Include(s => s.Sala)
                        .FirstOrDefaultAsync(s => s.Id == ticketDto.ShowtimeId);

                    if (showtime == null)
                        throw new Exception("Función no encontrada");

                    if (ticketDto.Asiento < 1 || ticketDto.Asiento > showtime.Sala.Capacidad)
                        throw new Exception("Asiento inválido");

                    bool asientoOcupado = await _context.Tickets
                        .AnyAsync(t => t.ShowtimeId == ticketDto.ShowtimeId
                                    && t.Asiento == ticketDto.Asiento);

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
                        VentaId = venta.Id,
                        Ticket = ticket,
                        Cantidad = 1,
                        PrecioUnitario = showtime.Price
                    });
                }

                venta.Total = total;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new CompraResponseDTO
                {
                    mensaje = "Compra realizada con éxito",
                    ventaId = venta.Id,
                    codigo = venta.Codigo,
                    total = venta.Total
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new ErrorResponseDTO
                {
                    error = ex.Message
                });
            }
        }
    }
}
