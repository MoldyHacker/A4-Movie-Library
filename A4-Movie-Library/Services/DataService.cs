using BetterConsoleTables;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using A4_Movie_Library.Models;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Models;
using Movie= A4_Movie_Library.Models.Movie;

namespace A4_Movie_Library.Services;

public class DataService : IDataService
{
    private readonly ILogger<IDataService> _logger;
    public DataModel DataModel { get; set; }
    public List<Movie> Movie { get; set; }

    private string _filePath = $"{Environment.CurrentDirectory}../../../../Data/movies.csv";

    private List<DataModel> _fileRecords;

    public DataService(ILogger<IDataService> logger)
    {
        _logger = logger;
        DataModel = new();
        Movie = new();

        DataModel.TitlesList = new List<string>();
    }
    

    public void Read()
    {
        _logger.LogInformation("Reading");
        try
        {
            StreamReader sr = new StreamReader(_filePath);
            // first line contains column headers
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                // first look for quote(") in string
                // this indicates a comma(,) in movie title
                int idx = line.IndexOf('"');
                if (idx == -1)
                {
                    // no quote = no comma in movie title
                    string[] movieDetails = line.Split(',');
                    DataModel.TitlesList.Add(movieDetails[1]);
                    Movie.Add(new Movie() { Id = (long)int.Parse(movieDetails[0]), Title = movieDetails[1], Genres = movieDetails[2].Replace("|", ", ") });
                    // Movie.Add(new Movie(int.Parse(movieDetails[0]), movieDetails[1], movieDetails[2].Replace("|",", ")));
                }
                else
                {
                    // quote = comma in movie title
                    // extract the movieId
                    int mId = int.Parse(line.Substring(0, idx - 1));
                    // _movieIds.Add(mId);
                    // remove movieId and first quote from string
                    line = line.Substring(idx + 1);
                    // find the next quote
                    idx = line.IndexOf('"');
                    // extract the movieTitle
                    // _movieTitles.Add(line.Substring(0, idx));
                    DataModel.TitlesList.Add(line.Substring(0, idx));
                    // remove title and last comma from the string
                    string genreLine = line.Substring(idx + 2).Replace("|", ", ");
                    // replace the "|" with ", "
                    // _movieGenres.Add(genreLine.Replace("|", ", "));

                    // Movie.Add(new Movie(){Id = (long)mId, Title = line.Substring(0,idx), Genres = genreLine});
                    // Movie.Add(new Movie(mId, line.Substring(0, idx), genreLine));
                }
            }
            // close file when done
            sr.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Movies in file {Count}", DataModel.TitlesList.Count);
    }

    public void Write(DataModel dataModelInput)
    {
        if (dataModelInput == null)
            return;

        var records = new List<DataModel> { dataModelInput };

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
            HasHeaderRecord = false
        };

        using var writer = new StreamWriter(_filePath, true);

        _logger.LogInformation("Writing data file");
        using (var csv = new CsvWriter(writer, config))
        {
            csv.Context.RegisterClassMap<DataModelMap>();
            csv.WriteRecords(records);
        }

        writer.Close();
    }

    public void Display()
    {
        // Create Table
        Table table = new Table("ID", "Movie Title", "Genre(s)");
        // loop thru Movie Lists
        foreach (var movie in Movie)
            table.AddRow(movie.Id, movie.Title, movie.Genres);

        
        Console.Write(table.ToString());
    }

    public int NextId()
    {
        int newId;

        try
        { 
            newId = int.Parse(File.ReadLines(_filePath).Last().Split(',')[0]) + 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogInformation("No data found. ID set to 1");
            newId = 1;
        }
        return newId;
    }

    public bool MatchTitle(string title)
    {
        if (DataModel.TitlesList.Count == 0)
            Read();
        return DataModel.TitlesList.ConvertAll(t => t.ToLower().Trim()).Contains(title.ToLower().Trim());
    }
}