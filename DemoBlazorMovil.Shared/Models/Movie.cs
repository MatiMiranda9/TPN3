using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DemoBlazorMovil.Shared.Models;


public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public int Year { get; set; }

    public string? ImagePath { get; set; }

    public bool IsActive = true;

    public List<Showtime> Showtimes { get; set; } = new();
}
