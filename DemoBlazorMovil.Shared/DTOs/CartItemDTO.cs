using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class CartItemDTO
    {
        public int ArticuloId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public string? ImagePath { get; set; }

        public decimal Subtotal => Precio * Cantidad;
    }
}
