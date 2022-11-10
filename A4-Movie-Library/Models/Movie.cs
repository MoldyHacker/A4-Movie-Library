namespace A4_Movie_Library.Models;

public class Movie : Media
{
    public string Genres { get; set; }

    public DateTime ReleaseDate { get; set; }

    public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    public virtual ICollection<UserMovie> UserMovies { get; set; }


    public Movie(int id, string title, string genres)
    {
        Id = id;
        Title = title;
        Genres = genres;
    }
}