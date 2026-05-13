using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoBlazorMovil.Shared.Models;

[Table("Venta")]
public partial class Venta
{
    public int Id { get; set; }

    public decimal Total { get; set; }

    public DateTime Fecha { get; set; }

    public int UserId { get; set; }

    public string Codigo { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();

    public virtual User User { get; set; } = null!;
}
