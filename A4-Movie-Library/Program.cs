using A4_Movie_Library.Services;
using Microsoft.Extensions.DependencyInjection;

namespace A4_Movie_Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
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