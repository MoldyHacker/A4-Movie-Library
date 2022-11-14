using System.Globalization;
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


    public MenuService(ILogger<IMenuService> logger, IUserService userService, IDatabaseService dataService)
    {
        _logger = logger;
        _userService = userService;
        _dataService = dataService;
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
                    Console.Write("Release Date: ");
                    _dataService.DataModel.ReleaseDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    _dataService.Write(_dataService.DataModel);
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
        } while (menuChoice != Menu.MenuOptions.Exit);

        menu.Exit();
    }
}