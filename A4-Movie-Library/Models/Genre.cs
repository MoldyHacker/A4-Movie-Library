namespace A4_Movie_Library.Models;

public class Genre
{
    public long Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<MovieGenre> MovieGenres { get; set; }
}