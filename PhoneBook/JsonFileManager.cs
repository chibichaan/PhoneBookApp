using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PhoneBook;

public class JsonFileManager : IFileManager
{
    private const string FILE_PATH_JSON = "ThePhoneBook.json";

    public Book Load()
    {
        if (!File.Exists(FILE_PATH_JSON))
        {
            File.WriteAllBytes(FILE_PATH_JSON, 
                JsonSerializer.SerializeToUtf8Bytes(new Book(), new JsonSerializerOptions{WriteIndented = true})); 
        }
        
        var json = File.ReadAllText(FILE_PATH_JSON);
        var book = string.IsNullOrEmpty(json) 
            ? new Book() 
            : JsonSerializer.Deserialize<Book>(json);
        
        return book;
    }

    public void Save(Book book)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var jsonSerialized = JsonSerializer.Serialize(book, options);
        File.WriteAllText(FILE_PATH_JSON, jsonSerialized);

    }
}