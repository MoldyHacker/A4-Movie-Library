using A4_Movie_Library.Models;

namespace A4_Movie_Library.Services;

public interface IDatabaseService
{
    DataModel DataModel { get; set; }
    void Read();
    void Write(DataModel dataModel);
    MovieGenre WriteGenres(DataModel dataModel);
    void Display();
    void Search(string title);
    void Update(string title);
    void Delete(string title);
    bool MatchTitle(string title);
    int NextId();
    
}