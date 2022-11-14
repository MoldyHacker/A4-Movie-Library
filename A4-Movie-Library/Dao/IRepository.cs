using A4_Movie_Library.Models;
using Movie = A4_Movie_Library.Models.Movie;

namespace A4_Movie_Library.Dao
{
    public interface IRepository
    {
        List<A4_Movie_Library.Models.Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);
    }
}
