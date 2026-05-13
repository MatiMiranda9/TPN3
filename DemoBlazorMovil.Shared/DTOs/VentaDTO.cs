using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class VentaDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public List<VentaArticuloDTO> Articulos { get; set; } = new();
        public List<VentaTicketDTO> Tickets { get; set; } = new();
    }
}
