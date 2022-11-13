using A4_Movie_Library.Models;
using Movie = MovieLibraryEntities.Models.Movie;

namespace A4_Movie_Library.Dao
{
    public interface IRepository
    {
        List<MovieLibraryEntities.Models.Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);
    }
}
