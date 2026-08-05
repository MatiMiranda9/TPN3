using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoBlazorMovil.Shared.Models;

[Table("DetalleVenta")]
public partial class DetalleVenta
{
    public int Id { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public int VentaId { get; set; }

    public int? ArticuloId { get; set; }

    public int? TicketId { get; set; }

    public virtual Articulo? Articulo { get; set; }

    public virtual Ticket? Ticket { get; set; }

    public virtual Venta Venta { get; set; } = null!;
}
