using System.Globalization;
using A4_Movie_Library.Models;
using Microsoft.Extensions.Logging;
using Spectre.Console;

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
}