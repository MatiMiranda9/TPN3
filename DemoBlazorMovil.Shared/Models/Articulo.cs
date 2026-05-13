using System;
using System.Collections.Generic;

namespace DemoBlazorMovil.Shared.Models;


public partial class Articulo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string Descripcion { get; set; } = null!;

    public int Stock { get; set; }

    public string Categoria { get; set; } = null!;

    public string? ImagePath { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
}
