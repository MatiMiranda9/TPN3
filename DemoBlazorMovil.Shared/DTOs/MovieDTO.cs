using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.DTOs
{
    public class ShowtimeDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Room { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public List<ShowtimeDto> Showtimes { get; set; } = new();
    }
}

