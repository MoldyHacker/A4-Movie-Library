using A4_Movie_Library.Models;
using BetterConsoleTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MovieLibraryEntities.Context;
// using MovieLibraryEntities.Models;
// using MovieGenre = A4_Movie_Library.Models.MovieGenre;

namespace A4_Movie_Library.Services;

public class DatabaseService : IDatabaseService
{
    private readonly ILogger<IDatabaseService> _logger;
    public DataModel DataModel { get; set; }


    public DatabaseService(ILogger<IDatabaseService> logger)
    {
        _logger = logger;
        DataModel = new ();
    }

    public void Read()
    {
        throw new NotImplementedException();
    }

    // public void Write(long id, string title, MovieGenre genre)
    // {
    //     throw new NotImplementedException();
    // }

    public void Write(DataModel dataModelInput)
    {

        foreach (var genre in dataModelInput.Genres)
        {
            
            using (var db = new MovieContext())
            {
                var genres = db.Genres;
                
                


            }
        }

        using (var db = new MovieContext())
        {
            var movie = new Movie(){ Title = DataModel.Title, };
        }
    }

    public MovieGenre WriteGenres(DataModel dataModel)
    {
        throw new NotImplementedException();
    }

    public void Display()
    {
        // Create Table
        Table table = new Table("ID", "Movie Title", "Genre(s)");
        // loop thru Movie Lists

        using (var db = new MovieContext())
        {
            var movies = db.Movies.ToList();
            foreach (var dbMovie in movies)
            {
                Console.WriteLine($"{dbMovie.Id}, {dbMovie.Title}");

                List<string> genreList = new List<string>();

                foreach (var dbMovieGenre in dbMovie.MovieGenres)
                    genreList.Add(dbMovieGenre.Genre.Name);

                var genres = String.Join(", ",genreList);
                table.AddRow(dbMovie.Id, dbMovie.Title, genres);
            }
        }

        Console.Write(table.ToString());
    }

    public void Search(string title)
    {
        throw new NotImplementedException();
    }

    public void Update(string title)
    {
        throw new NotImplementedException();
    }

    public void Delete(string title)
    {
        throw new NotImplementedException();
    }

    public int NextId()
    {
        int newId;

        try
        {
            using var db = new MovieContext();
            newId = (int)db.Movies.OrderBy(m=>m.Id).LastOrDefault()!.Id + 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogInformation("No data found. ID set to 1");
            newId = 1;
        }
        return newId;
    }

    public bool MatchTitle(string title)
    {
        try
        {
            using var db = new MovieContext();
            return db.Movies.Any(m => m.Title.ToLower().Equals(title.ToLower()));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogInformation("Match check failed");
            return false;
        }
    }
}