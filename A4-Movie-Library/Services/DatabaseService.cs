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

                    DataModel.Title = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter the movie title: ")
                            .ValidationErrorMessage("That movie is already entered.")
                            .Validate(input
                                => !MatchTitle(input)
                                    ? ValidationResult.Success()
                                    : ValidationResult.Error("That movie is already entered.")
                            ));

                    DataModel.Genres = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                            .Title("What are the genres associated with this movie?")
                            .PageSize(20)
                            .MoreChoicesText("(Move up and down to reveal more genres)")
                            .InstructionsText("[grey](Press [blue]<space>[/] to toggle a class, " +
                                              "[green]<enter>[/] to accept)[/]")
                            .AddChoices("Action", "Adventure", "Animation", "Children's", "Comedy", "Crime",
                                "Documentary", "Drama", "Fantasy", "Film-Noir", "Horror",
                                "Musical", "Mystery", "Romance", "Sci-Fi", "Thriller", "War", "Western"));

                    // _userService.PopulateChoices();
                    Console.Write("Release Date: ");
                    DataModel.ReleaseDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString(CultureInfo.InvariantCulture));

                    
                    List<MovieGenre> listMovieGenres = new List<MovieGenre>();

                    foreach (var s in DataModel.Genres!)
                    {
                        Genre genre = db.Genres.First(g => g.Name.Equals(s));
                        var movieGenre = new MovieGenre() { Genre = genre, Movie = updateMovie };
                        listMovieGenres.Add(movieGenre);
                    }

                    updateMovie.Title = DataModel.Title;
                    updateMovie.MovieGenres = listMovieGenres;
                    db.Movies.Update(updateMovie);
                    db.SaveChanges();


                    Console.WriteLine($"Updated movie: {updateMovie.Id}, {updateMovie.Title}");
                    updateMovie.MovieGenres.ToList().ForEach(genre => Console.WriteLine($"\t{genre.Genre.Name}"));
                }
                else
                    Console.WriteLine("Exiting update selection...");
                
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