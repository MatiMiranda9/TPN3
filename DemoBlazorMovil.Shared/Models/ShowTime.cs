using System;
using System.Collections.Generic;

namespace DemoBlazorMovil.Shared.Models;


public partial class Showtime
{
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public decimal Price { get; set; }

    public int MovieId { get; set; }

    public int SalaId { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual Movie Movie { get; set; } = null!;

    public virtual Sala Sala { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
