using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DemoBlazorMovil.Models;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(100, ErrorMessage = "El título no puede superar los 100 caracteres")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "El género es obligatorio")]
    [StringLength(50, ErrorMessage = "El género no puede superar los 50 caracteres")]
    public string Genre { get; set; } = "";

    [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
    public int Year { get; set; }

    public string? ImagePath { get; set; } = "images/movies/generico.jpg";

    public List<Showtime> Showtimes { get; set; } = new();
}

