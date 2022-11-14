using DataModel = A4_Movie_Library.Models.DataModel;
using MovieLibraryEntities.Models;

namespace A4_Movie_Library.Services;

public interface IDatabaseService
{
    DataModel DataModel { get; set; }
    void Read();
    void Write(DataModel dataModel);
    MovieGenre WriteGenres(DataModel dataModel);
    void Display();
    void Search(string title);
    void Update(long id);
    void Delete(long id);
    bool MatchTitle(string title);
    int NextId();
    
}