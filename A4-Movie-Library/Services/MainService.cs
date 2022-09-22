using Microsoft.Extensions.Logging;

namespace A4_Movie_Library.Services;

public class MainService : IMainService
{
    private readonly IMenuService _menuService;

    public MainService(IMenuService menuService)
    {
        _menuService = menuService;
    }
    public void Invoke()
    {
        _menuService.Invoke();
    }
}