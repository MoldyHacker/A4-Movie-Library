using A4_Movie_Library.Models;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace A4_Movie_Library.Services;

public class UserService : IUserService
{
    private readonly ILogger<IUserService> _logger;
    private readonly IDataService _dataService;

    public UserService(ILogger<IUserService> logger, IDataService dataService)
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
                .AddChoices("Action", "Adventure", "Animation", "Children", "Comedy", "Crime",
                    "Documentary", "Drama", "Fantasy", "Film-Noir", "Horror", "IMAX",
                    "Musical", "Mystery", "Romance", "Sci-Fi", "Thriller", "War", "Western"));
    }
}