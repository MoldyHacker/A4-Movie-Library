using Microsoft.Extensions.Logging;
namespace A4_Movie_Library.Services;

public class MenuService : IMenuService
{
    private readonly ILogger<MenuService> _logger;
    public MenuService(ILogger<MenuService> logger)
    {
        _logger = logger;
    }

    public MenuService()
    {

    }

    public void Invoke()
    {
        var menu = new Menu();
        var input = new UserService();
        var data = new DataService();

        Menu.MenuOptions menuChoice;
        do
        {
            menuChoice = menu.ChooseAction();

            switch (menuChoice)
            {
                case Menu.MenuOptions.Add:
                    input.PopulateChoices();
                    data.Write(input.DataModel);
                    break;
                case Menu.MenuOptions.Display:
                    data.Read();
                    data.Display();
                    break;
            }
        } while (menuChoice != Menu.MenuOptions.Exit);

        menu.Exit();
    }
}