using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.Models;

public class Showtime
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La hora de la función es obligatoria")]
    public DateTime Time { get; set; }

    [Required(ErrorMessage = "La sala es obligatoria")]
    [StringLength(20, ErrorMessage = "El nombre de la sala no puede superar los 20 caracteres")]
    public string Room { get; set; } = "";

    [Range(0, 50000.00, ErrorMessage = "El precio debe estar entre 0 y 50000")]
    public decimal Price { get; set; } = decimal.Zero;

    public int MovieId { get; set; }
    [JsonIgnore]
    public Movie? Movie { get; set; }
}
