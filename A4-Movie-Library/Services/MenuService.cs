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
                    _logger.LogInformation("add");
                    _userService.PopulateChoices();
                    _dataService.Write(_dataService.DataModel);
                    break;
                case Menu.MenuOptions.Display:
                    _dataService.Read();
                    _dataService.Display();
                    _logger.LogInformation("Read");
                    break;
            }
        } while (menuChoice != Menu.MenuOptions.Exit);

        menu.Exit();
    }
}