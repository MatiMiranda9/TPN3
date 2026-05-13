using System;
using System.Collections.Generic;

namespace DemoBlazorMovil.Shared.Models;


public partial class Sala
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Capacidad { get; set; }

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
