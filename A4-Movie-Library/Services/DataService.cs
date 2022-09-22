using BetterConsoleTables;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using A4_Movie_Library.Models;
using Microsoft.Extensions.Logging;

namespace A4_Movie_Library.Services;

public class DataService : IDataService
{
    private readonly ILogger<IDataService> _logger;
    public DataModel DataModel { get; set; }

    private string _filePath;

    private List<int> _movieIds;
    private List<string> _movieTitles;
    private List<string> _movieGenres;

    private List<DataModel> _fileRecords;

    public DataService(ILogger<IDataService> logger)
    {
        _logger = logger;
        _filePath = $"{Environment.CurrentDirectory}../../../../Data/movies.csv";
        DataModel = new();

        _movieIds = new List<int>();
        _movieTitles = new List<string>();
        _movieGenres = new List<string>();
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
                    // movie details are separated with comma(,)
                    string[] movieDetails = line.Split(',');
                    // 1st array element contains movie id
                    _movieIds.Add(int.Parse(movieDetails[0]));
                    // 2nd array element contains movie title
                    _movieTitles.Add(movieDetails[1]);
                    DataModel.TitlesList.Add(movieDetails[1]);
                    // 3rd array element contains movie genre(s)
                    // replace "|" with ", "
                    _movieGenres.Add(movieDetails[2].Replace("|", ", "));
                }
                else
                {
                    // quote = comma in movie title
                    // extract the movieId
                    _movieIds.Add(int.Parse(line.Substring(0, idx - 1)));
                    // remove movieId and first quote from string
                    line = line.Substring(idx + 1);
                    // find the next quote
                    idx = line.IndexOf('"');
                    // extract the movieTitle
                    _movieTitles.Add(line.Substring(0, idx));
                    DataModel.TitlesList.Add(line.Substring(0, idx));
                    // remove title and last comma from the string
                    line = line.Substring(idx + 2);
                    // replace the "|" with ", "
                    _movieGenres.Add(line.Replace("|", ", "));
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
        for (int i = 0; i < _movieIds.Count; i++)
            table.AddRow(_movieIds[i], _movieTitles[i], _movieGenres[i]);
        
        Console.Write(table.ToString());
    }

    public string NextId()
    {
        string newId;

        try
        { 
            newId = (int.Parse(File.ReadLines(_filePath).Last().Split(',')[0]) + 1).ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogInformation("No data found. ID set to 1");
            newId = "1";
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