using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class ConfirmarCompraDTO
    {
        public int UserId { get; set; }

        public List<ArticuloCompraDTO> Articulos { get; set; } = new();

        public List<TicketCompraDTO> Tickets { get; set; } = new();
    }
}
