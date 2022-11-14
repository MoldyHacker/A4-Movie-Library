using MovieLibraryEntities.Models;
using DataModel = A4_Movie_Library.Models.DataModel;
using BetterConsoleTables;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MovieLibraryEntities.Context;
using Movie = MovieLibraryEntities.Models.Movie;
using Genre = MovieLibraryEntities.Models.Genre;

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

    public void Write(DataModel dataModelInput)
    {
        long movieId;
        using (var db = new MovieContext())
        {
            var movie = new Movie()
            {
                Title = dataModelInput.Title,
                ReleaseDate = dataModelInput.ReleaseDate,

            };
            db.Movies.Add(movie);
            db.SaveChanges();

            var newMovie = db.Movies.FirstOrDefault(m => m.Title!.Equals(dataModelInput.Title));
            Console.WriteLine($"{newMovie!.Id} {newMovie.Title}");

            List<Genre> genreList = new List<Genre>();

            // ICollection<MovieGenre> movieGenres = new List<MovieGenre>();

            foreach (var s in dataModelInput.Genres!)
            {
                Genre genre = db.Genres.First(g=>g.Name.Equals(s));
                var movieGenre = new MovieGenre() { Genre = genre, Movie = newMovie };
                // movieGenres.Add(movieGenre);
                db.MovieGenres.Add(movieGenre);
                db.SaveChanges();
            }
            

            // var genres = new MovieGenre()
            // {
            //
            // }
        }
        



        // List<long> genreIds = new List<long>();
        // foreach (var genre in dataModelInput.Genres)
        // {
        //     using (var db = new MovieContext())
        //     {
        //         genreIds.Add(db.Genres.FirstOrDefault(g => g.Name.Equals(genre))!.Id);
        //     }
        // }
        //
        // MovieGenre movieGenre = new MovieGenre()
        // {
        //
        // }

        
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
            newId = (int)db.Movies.OrderBy(m=>m.Id).LastOrDefault().Id + 1;
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