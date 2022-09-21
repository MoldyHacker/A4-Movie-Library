using Microsoft.Extensions.Logging;

namespace A4_Movie_Library.Services;

public class MainService : IMainService
{
    public void Invoke()
    {
        var menu = new MenuService();
        menu.Invoke();
    }
}