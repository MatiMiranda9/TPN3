using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class ShowtimeDTO
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public decimal Price { get; set; }

        public int SalaId { get; set; }

        public string? SalaNombre { get; set; }

        public int MovieId { get; set; }

        public string MovieNombre { get; set; } = "";

        public string ImagePath { get; set; } = "";

        public bool IsActive { get; set; } = true;
    }

}
