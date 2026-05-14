using System;
using System.Collections.Generic;

namespace DemoBlazorMovil.Shared.Models;


public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ImagePath { get; set; }

    public int IdRol { get; set; }

    public bool IsActive { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
