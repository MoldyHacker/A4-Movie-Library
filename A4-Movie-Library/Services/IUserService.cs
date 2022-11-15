using A4_Movie_Library.Models;

namespace A4_Movie_Library.Services;

public interface IUserService
{
    void PopulateChoices();
    DataModel Populate();
}