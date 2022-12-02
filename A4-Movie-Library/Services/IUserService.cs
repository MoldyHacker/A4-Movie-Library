using A4_Movie_Library.Models;
using MovieLibraryEntities.Models;

namespace A4_Movie_Library.Services;

public interface IUserService
{
    void PopulateChoices();
    DataModel Populate();
    User PopulateUser();
    UserMovie PopulateUserMovie();
}