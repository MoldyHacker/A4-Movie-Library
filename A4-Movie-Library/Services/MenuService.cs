using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using A4_Movie_Library.Models;

namespace A4_Movie_Library.Services;

public class MenuService : IMenuService
{
    private readonly ILogger<IMenuService> _logger;
    private readonly IUserService _userService;
    private readonly IDatabaseService _dataService;
    // private readonly IDatabaseService _databaseService;


    public MenuService(ILogger<IMenuService> logger, IUserService userService, IDatabaseService dataService)
    {
        _logger = logger;
        _userService = userService;
        _dataService = dataService;
        // _databaseService = databaseService;
    }

    public void Invoke()
    {
        var menu = new Menu();

        Menu.MenuOptions menuChoice;
        do
        {
            menuChoice = menu.ChooseAction();

            switch (menuChoice)
            {
                case Menu.MenuOptions.Add:
                    _logger.LogInformation("Add");
                    _userService.PopulateChoices();
                    _dataService.Write(_dataService.DataModel);
                    // _databaseService.Write(_dataService.DataModel);

                    break;




                case Menu.MenuOptions.Display:
                    _logger.LogInformation("Display");
                    Console.Write("With genres appended?(y/n): ");
                    using (var db = new MovieContext()) 
                    {
                        var movies = db.Movies.ToList();
                        switch (Console.ReadLine()?.ToLower())
                        {
                            case "y":
                                foreach (var dbMovie in movies)
                                {
                                    Console.WriteLine($"{dbMovie.Id}, {dbMovie.Title}");
                                    foreach (var dbMovieGenre in dbMovie.MovieGenres)
                                        Console.WriteLine($"\t{dbMovieGenre.Genre.Name}");
                                }
                                break;
                            case "n":
                                foreach (var dbMovie in movies) 
                                    Console.WriteLine($"{dbMovie.Id}, {dbMovie.Title}");
                                break;
                            default:
                                _logger.LogError("Input not available.");
                                break;
                        }
                    }

                    _dataService.Display();
                    // _dataService.Read();
                    // _dataService.Display();
                    break;




                case Menu.MenuOptions.Search:
                    _logger.LogInformation("Search");
                    break;




                case Menu.MenuOptions.Update:
                    _logger.LogInformation("Update");
                    break;    
                



                case Menu.MenuOptions.Delete:
                    _logger.LogInformation("Update");
                    break;
            }
        } while (menuChoice != Menu.MenuOptions.Exit);

        menu.Exit();
    }
}