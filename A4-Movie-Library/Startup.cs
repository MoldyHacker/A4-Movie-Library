﻿using A4_Movie_Library.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace A4_Movie_Library;

public class Startup
{
    public IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddFile("app.log");
        });

        // Add new lines of code here to register any interfaces and concrete services you create
        services.AddSingleton<IMainService, MainService>();
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IUserService, UserService>();

        return services.BuildServiceProvider();
    }
}