using A4_Movie_Library.Models;

namespace A4_Movie_Library.Services;

public interface IDataService
{
    DataModel DataModel { get; set; }
    void Read();
    void Write(DataModel dataModelInput);
    void Display();
    bool MatchTitle(string title);
    int NextId();
}