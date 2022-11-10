using Microsoft.Extensions.Logging;
namespace A4_Movie_Library.Services;

public class MenuService : IMenuService
{
    private readonly ILogger<IMenuService> _logger;
    private readonly IUserService _userService;
    private readonly IDataService _dataService;

    public MenuService(ILogger<IMenuService> logger, IUserService userService, IDataService dataService)
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
                    _dataService.Write(_dataService.DataModel);
                    break;
                case Menu.MenuOptions.Display:
                    _logger.LogInformation("Read");
                    _dataService.Read();
                    _dataService.Display();
                    break;
                case Menu.MenuOptions.Search:
                    _logger.LogInformation("Search");
                    break;
                case Menu.MenuOptions.Update:
                    _logger.LogInformation("Update");
                    break;
            }
        } while (menuChoice != Menu.MenuOptions.Exit);

        menu.Exit();
    }
}