using A4_Movie_Library.Models;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace A4_Movie_Library.Services;

public class UserService : IUserService
{
    private readonly ILogger<IUserService> _logger;

    public DataModel DataModel { get; set; }
    private DataModel _dataModel = new();

    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
    }

    public UserService()
    {
        DataModel = _dataModel;
    }


    public void PopulateChoices()
    {
        var data = new DataService();

        _dataModel.Id = data.NextId();

        _dataModel.Title = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the movie title: ")
                .ValidationErrorMessage("That movie is already entered.")
                .Validate(input 
                => !data.MatchTitle(input) 
                    ? ValidationResult.Success() 
                    : ValidationResult.Error("That movie is already entered.")
                ));

        _dataModel.Genres = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("What are the genres associated with this movie?")
                .PageSize(20)
                .MoreChoicesText("(Move up and down to reveal more genres)")
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a class, " +
                                  "[green]<enter>[/] to accept)[/]")
                .AddChoices("Action", "Adventure", "Animation", "Children", "Comedy", "Crime",
                    "Documentary", "Drama", "Fantasy", "Film-Noir", "Horror", "IMAX",
                    "Musical", "Mystery", "Romance", "Sci-Fi", "Thriller", "War", "Western"));
    }
}