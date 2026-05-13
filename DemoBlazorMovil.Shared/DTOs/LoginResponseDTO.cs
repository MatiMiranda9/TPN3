using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;

        public UserDto User { get; set; } = new();
    }
}
