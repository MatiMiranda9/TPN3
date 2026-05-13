using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class AsientosDisponibilidadDTO
    {
        public int Capacidad { get; set; }
        public List<int> Ocupados { get; set; } = new();
    }
}
