using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class SalaDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sala")]
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public int Capacidad { get; set; }
    }
}
