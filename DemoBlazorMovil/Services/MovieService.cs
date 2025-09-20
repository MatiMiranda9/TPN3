using DemoBlazorMovil.Models;

namespace DemoBlazorMovil.Services;

public class MovieService
{
    private readonly List<Movie> _movies = new()
    {
        new Movie
        {
            Id = 1,
            Title = "Inception",
            Genre = "Sci-Fi",
            Year = 2010,
            ImagePath = "images/movies/inception.jpg",
            Showtimes = new List<Showtime>
            {
                new Showtime { Id = 1, Time = DateTime.Today.AddHours(18), Room = "1", Price = 1200, MovieId = 1 },
                new Showtime { Id = 2, Time = DateTime.Today.AddHours(21), Room = "1", Price = 1200, MovieId = 1 }
            }
        },
        new Movie
        {
            Id = 2,
            Title = "Interstellar",
            Genre = "Sci-Fi",
            Year = 2014,
            ImagePath = "images/movies/interstellar.jpg",
            Showtimes = new List<Showtime>
            {
                new Showtime { Id = 3, Time = DateTime.Today.AddHours(19), Room = "2", Price = 1500, MovieId = 2 }
            }
        }
    };

    private int NextId => _movies.Count == 0 ? 1 : _movies.Max(m => m.Id) + 1;
    private int NextShowtimeId => _movies.SelectMany(m => m.Showtimes).Any()
        ? _movies.SelectMany(m => m.Showtimes).Max(s => s.Id) + 1 : 1;

    // 📌 Películas
    public IReadOnlyList<Movie> GetAll() => _movies.OrderBy(m => m.Id).ToList();

    public Movie? GetById(int id) => _movies.FirstOrDefault(m => m.Id == id);

    public Movie Add(Movie movie)
    {
        movie.Id = NextId;

        // cada showtime nuevo debe tener su MovieId y Id único
        foreach (var st in movie.Showtimes)
        {
            st.Id = NextShowtimeId;
            st.MovieId = movie.Id;
        }

        _movies.Add(movie);
        return movie;
    }

    public bool Update(Movie movie)
    {
        var existing = _movies.FirstOrDefault(m => m.Id == movie.Id);
        if (existing == null) return false;

        // Actualizamos propiedades básicas
        existing.Title = movie.Title;
        existing.Genre = movie.Genre;
        existing.Year = movie.Year;
        existing.ImagePath = movie.ImagePath;

        // Actualizamos funciones (manteniendo MovieId)
        existing.Showtimes = movie.Showtimes.Select(s => new Showtime
        {
            Id = s.Id != 0 ? s.Id : NextShowtimeId, // si no tiene Id, generamos uno nuevo
            Time = s.Time,
            Room = s.Room,
            Price = s.Price,
            MovieId = existing.Id
        }).ToList();

        return true;
    }

    public bool Delete(int id)
    {
        var removed = _movies.RemoveAll(m => m.Id == id);
        return removed > 0;
    }

    // 📌 Funciones
    public Showtime AddShowtime(int movieId, Showtime showtime)
    {
        var movie = GetById(movieId);
        if (movie == null) throw new Exception("Película no encontrada");

        showtime.Id = NextShowtimeId;
        showtime.MovieId = movieId;

        movie.Showtimes.Add(showtime);
        return showtime;
    }

    public bool UpdateShowtime(int movieId, Showtime showtime)
    {
        var movie = GetById(movieId);
        if (movie == null) return false;

        var idx = movie.Showtimes.FindIndex(s => s.Id == showtime.Id);
        if (idx == -1) return false;

        showtime.MovieId = movieId; // aseguramos relación
        movie.Showtimes[idx] = showtime;
        return true;
    }

    public bool DeleteShowtime(int movieId, int showtimeId)
    {
        var movie = GetById(movieId);
        if (movie == null) return false;

        var removed = movie.Showtimes.RemoveAll(s => s.Id == showtimeId);
        return removed > 0;
    }
}
