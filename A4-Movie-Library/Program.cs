// using A4_Movie_Library.Context;
using A4_Movie_Library.Services;
using Microsoft.Extensions.DependencyInjection;
using MovieLibraryEntities.Context;

namespace A4_Movie_Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var context = new MovieContext();
                var movies = context.Movies;

                foreach (var movie in movies)
                {
                    Console.WriteLine(movie.Title);
                }


                var startup = new Startup();
                var serviceProvider = startup.ConfigureServices();
                var service = serviceProvider.GetService<IMainService>();

                service?.Invoke();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}