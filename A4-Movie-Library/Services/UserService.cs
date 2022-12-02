using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using A4_Movie_Library.Models;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using Spectre.Console;
using Movie = MovieLibraryEntities.Models.Movie;

namespace A4_Movie_Library.Services;

public class UserService : IUserService
{
    private readonly ILogger<IUserService> _logger;
    private readonly IDatabaseService _dataService;


    public UserService(ILogger<IUserService> logger, IDatabaseService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    public void PopulateChoices()
    {
        _dataService.DataModel.Id = _dataService.NextId();

        _dataService.DataModel.Title = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the movie title: ")
                .ValidationErrorMessage("That movie is already entered.")
                .Validate(input 
                => !_dataService.MatchTitle(input)
                    ? ValidationResult.Success() 
                    : ValidationResult.Error("That movie is already entered.")
                ));

        _dataService.DataModel.Genres = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("What are the genres associated with this movie?")
                .PageSize(20)
                .MoreChoicesText("(Move up and down to reveal more genres)")
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a class, " +
                                  "[green]<enter>[/] to accept)[/]")
                .AddChoices("Action", "Adventure", "Animation", "Children's", "Comedy", "Crime",
                    "Documentary", "Drama", "Fantasy", "Film-Noir", "Horror",
                    "Musical", "Mystery", "Romance", "Sci-Fi", "Thriller", "War", "Western"));
    }

    public DataModel Populate()
    {
        var title = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the movie title: ")
                .ValidationErrorMessage("[red]That movie is already entered[/]")
                .Validate(input
                    => !_dataService.MatchTitle(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]That movie is already entered[/]")
                ));

       var genres = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("What are the genres associated with this movie?")
                .PageSize(20)
                .MoreChoicesText("(Move up and down to reveal more genres)")
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a class, " +
                                  "[green]<enter>[/] to accept)[/]")
                .AddChoices("Action", "Adventure", "Animation", "Children's", "Comedy", "Crime",
                    "Documentary", "Drama", "Fantasy", "Film-Noir", "Horror",
                    "Musical", "Mystery", "Romance", "Sci-Fi", "Thriller", "War", "Western"));

       var releaseDate = AnsiConsole.Prompt(
           new TextPrompt<string>("Enter the release date: ")
               .ValidationErrorMessage("[red]Date out of range[/]")
               .Validate(input 
                   => DateTime.TryParse(input, out DateTime result) 
                      && DateTime.Parse(input) <= DateTime.Now
                       ? ValidationResult.Success()
                       : ValidationResult.Error("[red]Not a valid date or in range[/]"))
               
                );

       var rDate = DateTime.Parse(releaseDate);

       return new DataModel() { Title = title, Genres = genres, ReleaseDate = rDate };
    }

    public User PopulateUser()
    {
        var age = AnsiConsole.Prompt(
            new TextPrompt<long>("Enter the users age")
                .ValidationErrorMessage("[red]That age is invalid[/]")
                .Validate(input =>
                {
                    return input switch
                    {
                        < 0 => ValidationResult.Error("[red]User age must be at least 0[/]"),
                        > 123 => ValidationResult.Error("[red]Invalid entry[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        var gender = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Enter the users gender")
                .AddChoices(new[]
                {
                    "Male", "Female",
                }))[0].ToString();

        var zipcode = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the users Zipcode")
                .ValidationErrorMessage("[red]That Zipcode is invalid[/]")
                .Validate(input =>
                {
                    return int.Parse(input) switch
                    {
                        
                        < 00500 => ValidationResult.Error("[red]Zipcode must be 5 digits and no lower than 00501[/]"),
                        > 99999 => ValidationResult.Error("[red]Zipcode can't be larger than 99999[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));

        var occupationName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select menu option")
                .AddChoices(new[]
                {
                    "Administrator",
                    "Artist",
                    "Doctor",
                    "Educator",
                    "Engineer",
                    "Entertainment",
                    "Executive",
                    "Healthcare",
                    "Homemaker",
                    "Lawyer",
                    "Librarian",
                    "Marketing",
                    "None",
                    "Other",
                    "Programmer",
                    "Retired",
                    "Salesman",
                    "Scientist",
                    "Student",
                    "Technician",
                    "Writer",
                }));

        long occupationId;
        using (var db = new MovieContext()) 
            occupationId = db.Occupations.FirstOrDefault(o => o.Name!.Equals(occupationName))!.Id;
        var occupation = new Occupation() { Name = occupationName };

        return new User() { Age = age, Gender = gender, ZipCode = zipcode, Occupation = occupation};
    }

    public UserMovie PopulateUserMovie()
    {
        var movieId = AnsiConsole.Prompt(
            new TextPrompt<long>("Enter the movie ID")
                .ValidationErrorMessage("[red]That ID is invalid[/]")
                .Validate(input 
                    => _dataService.MatchMovieId(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]That ID is invalid[/]")
                        ));
        var movie = _dataService.ReturnMovie(movieId);
        _dataService.Search(movie.Title);
        Console.WriteLine("");

        var retrievedMovie = new Movie()
            { Title = movie.Title, MovieGenres = movie.MovieGenres, ReleaseDate = movie.ReleaseDate };


        var userId = AnsiConsole.Prompt(
            new TextPrompt<long>("Enter the user ID")
                .ValidationErrorMessage("[red]That ID is invalid[/]")
                .Validate(input
                    => _dataService.MatchUserId(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]That ID is invalid[/]")
                        ));
        var user = _dataService.ReturnUser(userId);

        var rating = AnsiConsole.Prompt(
            new TextPrompt<long>("Enter the movies rating")
                .ValidationErrorMessage("[red]That rating is invalid[/] enter a rating from 0 - 5")
                .Validate(input =>
                {
                    return input switch
                    {

                        < 0 => ValidationResult.Error("[red]That rating is invalid[/] rating can't be lower than 0"),
                        > 5 => ValidationResult.Error("[red]That rating is invalid[/] rating can't be higher than 5"),
                        _ => ValidationResult.Success(),
                    };
                }));

        return new UserMovie() { Movie = movie, User = user, Rating = rating, RatedAt = DateTime.Now};
    }
}