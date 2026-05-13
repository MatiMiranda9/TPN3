using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class ArticuloDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "El precio es obligatorio")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; } = null!;
        public int Stock { get; set; } = 0;
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public string Categoria { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public string? ImagePath { get; set; }
    }
}
