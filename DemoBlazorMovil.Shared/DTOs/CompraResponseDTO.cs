using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class CompraResponseDTO
    {
        public string mensaje { get; set; }
        public int ventaId { get; set; }
        public string codigo { get; set; }
        public decimal total { get; set; }
    }

    public class ErrorResponseDTO
    {
        public string error { get; set; }
    }
}
