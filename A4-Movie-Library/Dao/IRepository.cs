using A4_Movie_Library.Models;

namespace A4_Movie_Library.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);
    }
}
