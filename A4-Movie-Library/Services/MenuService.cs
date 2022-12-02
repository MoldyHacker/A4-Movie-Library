using System.Globalization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using A4_Movie_Library.Models;
using Spectre.Console;

namespace A4_Movie_Library.Services;

public class MenuService : IMenuService
{
    private readonly ILogger<IMenuService> _logger;
    private readonly IDatabaseService _dataService;
    private readonly IUserDataService _userDataService;
    
    // private readonly IRepository _repo;


    public MenuService(ILogger<IMenuService> logger, IDatabaseService dataService, IUserDataService userDataService)
    {
        _logger = logger;
        _dataService = dataService;
        _userDataService = userDataService;
    }

    public void Invoke()
    {
        ILogger<IUserService> ILogger = null;
        IUserService userService = new UserService(ILogger, _dataService);
        var menu = new Menu();

        Menu.MenuOptions menuChoice;

        string mainMenu = "";
        do
        {
            // mainMenu = menu.ChooseInitialAction();
            mainMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select menu option")
                    .AddChoices(new []
                    {
                        "Movies", "Users", "Exit"
                    }));

            switch (mainMenu)
            {
                case "Movies":
                    do
                    {
                        _logger.LogInformation("Movies Section");
                        menuChoice = menu.ChooseAction();

                        switch (menuChoice)
                        {
                            case Menu.MenuOptions.Add:
                                _logger.LogInformation("Add");
                                _dataService.Write(userService.Populate());
                                break;

                            case Menu.MenuOptions.Display:



                                _logger.LogInformation("Display");
                                _dataService.Display();
                                break;

                            case Menu.MenuOptions.Search:
                                _logger.LogInformation("Search");
                                Console.Write("Search for movie title: ");
                                _dataService.Search(Console.ReadLine());
                                break;

                            case Menu.MenuOptions.Update:
                                _logger.LogInformation("Update");
                                Console.WriteLine("Enter the movie ID you would like to update: ");
                                _dataService.Update(long.Parse(Console.ReadLine()));
                                break;    
                            
                            case Menu.MenuOptions.Delete:
                                _logger.LogInformation("Update");
                                Console.Write("Enter the movie ID you would like to delete: ");
                                _dataService.Delete(long.Parse(Console.ReadLine()));
                                break;
                        }
                    } while (menuChoice != Menu.MenuOptions.Back);
                    break;

                case "Users":
                    _logger.LogInformation("User Section");
                    string userMenu = "";
                    do
                    {
                        userMenu = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select menu option")
                            .AddChoices(new[]
                            {
                                "Add User", "Add Rating", "Back"
                            }));
                        switch (userMenu)
                        {
                            case "Add User":
                                _logger.LogInformation("Add User");
                                _dataService.WriteUser(userService.PopulateUser());
                                break;
                            case "Add Rating":
                                _logger.LogInformation("Add Rating");
                                _dataService.WriteMovieRating(userService.PopulateUserMovie());
                                break;
                        }

                    } while (userMenu != "Back");
                    break;
            }
        } while (mainMenu != "Exit");

        menu.Exit();
    }
}