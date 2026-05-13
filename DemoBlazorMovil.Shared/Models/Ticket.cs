using System;
using System.Collections.Generic;

namespace DemoBlazorMovil.Shared.Models;


public partial class Ticket
{
    public int Id { get; set; }

    public int Asiento { get; set; }

    public int ShowtimeId { get; set; }

    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();

    public virtual Showtime Showtime { get; set; } = null!;
}
