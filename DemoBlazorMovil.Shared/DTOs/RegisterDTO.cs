using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="El nombre de usuario es obligatorio.")]
        [Length(4, 15, ErrorMessage = "El nombre de usuario debe tener entre 4 y 15 caracteres")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage ="El Email es obligatorio.")]
        [EmailAddress(ErrorMessage ="Formato de Email no valido.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage ="La contraseña es obligatoria.")]
        [Length(4, 12, ErrorMessage ="La contraseña debe tener entre 4 y 12 caracteres") ]
        public string Password { get; set; } = "";
    }
}
