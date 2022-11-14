using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace A4_Movie_Library.Models
{
    public class DataModel
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public List<string>? Genres { get; set; }
        public List<string>? TitlesList { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class ListConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var list = new List<string>();
            var array = text.Split('|');

            foreach (var s in array)
            {
                list.Add(s);
            }

            return list;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return string.Join('|', (List<string>)value);
        }
    }

    public class DataModelMap : ClassMap<DataModel>
    {
        public DataModelMap()
        {
            Map(m => m.Id).Name("movieId");
            Map(m => m.Title).Name("title");
            Map(m => m.Genres).Name("genres").TypeConverter<ListConverter>();
        }
    }
}
