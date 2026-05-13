using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class CartTicketDTO
    {
        public int ShowtimeId { get; set; }
        public string MovieNombre { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string Sala { get; set; } = "";
        public int Asiento { get; set; }

        public decimal Subtotal => Precio;
    }
}
