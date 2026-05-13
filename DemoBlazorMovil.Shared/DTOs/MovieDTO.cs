using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public int Year { get; set; }

        public string? Genre { get; set; }

        public string? ImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public List<ShowtimeDTO> Showtimes { get; set; } = new();
    }
}
