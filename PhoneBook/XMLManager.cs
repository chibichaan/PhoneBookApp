using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace PhoneBook;

public class XMLManager : IFileManager
{
    private const string FILE_PATH = "ThePhoneBook.xml";
    
    /// <summary>
    /// Выгрузка из xml файла информации
    /// </summary>
    /// <returns></returns>
    public Book Load()
    {
        if (!File.Exists(FILE_PATH))
        {
           File.WriteAllBytes(FILE_PATH, Array.Empty<byte>()); 
        }
        var xDoc = XDocument.Load(FILE_PATH);
        
        var book = new Book();
        
        var rows = xDoc.XPathSelectElements("//Book/Rows/*");

        foreach (var r in rows)
        {
            var row = new Row();
            row.Id = Guid.Parse(r.Element("Id").Value);
            row.PhoneNumber = r.Element("PhoneNumber").Value;
            row.FIO = r.Element(nameof(row.FIO)).Value;
            row.City = r.Element(nameof(row.City))?.Value;
            row.Street = r.Element(nameof(row.Street))?.Value;
            row.House = r.Element(nameof(row.House))?.Value;
            row.Apartment = r.Element(nameof(row.Apartment))?.Value;
            book.AddRow(row);
        }
        
        return book;
    }

    /// <summary>
    /// Сохранение в файл xml полученной информации
    /// </summary>
    /// <param name="book"></param>
    public void Save(Book book)
    {
        var xmlSer = new XmlSerializer(typeof(Book));
        using (var writer = new Utf8StringWriter())
        {
         xmlSer.Serialize(writer, book);
         File.WriteAllText(FILE_PATH, writer.ToString(), Encoding.UTF8);
        }
    }
}