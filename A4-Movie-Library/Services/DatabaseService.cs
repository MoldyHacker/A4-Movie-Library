using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MovieLibraryEntities.Models;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MovieLibraryEntities.Context;
using Spectre.Console;
using DataModel = A4_Movie_Library.Models.DataModel;
using Movie = MovieLibraryEntities.Models.Movie;
using Table = BetterConsoleTables.Table;
using A4_Movie_Library.Models;

// using Genre = MovieLibraryEntities.Models.Genre;

namespace A4_Movie_Library.Services;

public class DatabaseService : IDatabaseService
{
    private readonly ILogger<IDatabaseService> _logger;
    private readonly IUserService _userService;
    public DataModel DataModel { get; set; }


    public DatabaseService(ILogger<IDatabaseService> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
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

            var newMovie = db.Movies.First(m => m.Title!.Equals(dataModelInput.Title));
            

            List<Genre> genreList = new List<Genre>();

            foreach (var s in dataModelInput.Genres!)
            {
                Genre genre = db.Genres.First(g=>g.Name.Equals(s));
                var movieGenre = new MovieGenre() { Genre = genre, Movie = newMovie };
                db.MovieGenres.Add(movieGenre);
            }
            db.SaveChanges();

            Console.WriteLine("New Movie Added:");
            Console.WriteLine($"{newMovie!.Id}, {newMovie.Title}");

            foreach (var genre in newMovie.MovieGenres)
                Console.WriteLine($"\t{genre.Genre.Name}");
        }
    }

    public MovieGenre WriteGenres(DataModel dataModel)
    {
        throw new NotImplementedException();
    }

    public void Display()
    {
        Console.WriteLine("Gathering movies... May take some time");
        // Create Table
        Table table = new Table("ID", "Movie Title", "Genre(s)");
        // loop thru Movie Lists

        using (var db = new MovieContext())
        {
            var movies = db.Movies.ToList();
            foreach (var dbMovie in movies)
            {
                List<string> genreList = new List<string>();

                foreach (var dbMovieGenre in dbMovie.MovieGenres)
                    genreList.Add(dbMovieGenre.Genre.Name);

                var genres = String.Join(", ",genreList);

                table.AddRow(dbMovie.Id, dbMovie.Title, genres);
            }
        }

        Console.Write(table.ToString());
    }

    public void Search(string? title)
    {
        using (var db = new MovieContext())
        {
            var movies = db.Movies.ToList();
            var listMovies = movies.FindAll(m => m.Title.ToLower().Contains(title.ToLower()));
            foreach (var movie in listMovies)
            {
                Console.WriteLine($"{movie.Id}, {movie.Title}");
                movie.MovieGenres.ToList().ForEach(genre => Console.WriteLine($"\t{genre.Genre.Name}"));
            }
        }
    }

    public void Update(long id)
    {
        using (var db = new MovieContext())
        {
            var movies = db.Movies.ToList();
            var updateMovie = movies.FirstOrDefault(m => m.Id.Equals(id));
            if (updateMovie != null)
            {
                Console.WriteLine($"Found: {updateMovie.Id}, {updateMovie.Title}");
                updateMovie.MovieGenres.ToList().ForEach(genre => Console.WriteLine($"\t{genre.Genre.Name}"));
                Console.WriteLine("Is this correct?(y/n): ");
                if (Console.ReadLine().ToLower().Contains("y"))
                {
                    _userService.PopulateChoices();
                    Console.Write("Release Date: ");
                    DataModel.ReleaseDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    



                    // string userSelection;
                    // do
                    // {
                    //     Console.WriteLine("What would you like to update?" +
                    //                       "1. Title" +
                    //                       "2. Genres" +
                    //                       "3. Exit" +
                    //                       "> ");
                    //     userSelection = Console.ReadLine();
                    //     switch (userSelection)
                    //     {
                    //         case "1":
                    //             break;
                    //         case "2":
                    //             break;
                    //         case "3":
                    //             break;
                    //         default:
                    //             Console.WriteLine("Input not recognized");
                    //             break;
                    //     }
                    // } while (userSelection != "3");
                    

                }
                else
                {
                    Console.WriteLine("Exiting update selection...");
                }
            }
            else
            {
                _logger.LogError("ID not present for update");
                Console.WriteLine("ID not present");
            }
        }
    }

    public void Delete(long id)
    {
        using (var db = new MovieContext())
        {
            var movies = db.Movies.ToList();
            var deleteMovie = movies.FirstOrDefault(m => m.Id.Equals(id));
            if (deleteMovie != null)
            {
                Console.WriteLine($"Found: {deleteMovie.Id}, {deleteMovie.Title}");
                Console.WriteLine("Is this movie correct?(y/n): ");
                if (Console.ReadLine().ToLower().Contains("y"))
                {
                    db.Movies.Remove(deleteMovie);
                    db.SaveChanges();
                    Console.WriteLine($"{deleteMovie.Title} Successfully deleted");
                }
                else
                    Console.WriteLine("Exiting deletion selection... ");
            }
            else
            {
                _logger.LogError("ID not present for deletion");
                Console.WriteLine("ID not present");
            }
        }
    }

    public int NextId()
    {
        // int newId;

        // try
        // {
        //     using var db = new MovieContext();
        //     newId = (int)db.Movies.OrderBy(m=>m.Id).LastOrDefault().Id + 1;
        // }
        // catch (Exception ex)
        // {
        //     _logger.LogError(ex.Message);
        //     _logger.LogInformation("No data found. ID set to 1");
        //     newId = 1;
        // }
        // return newId;
        return 1;
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