namespace PhoneBook;

public interface IFileManager
{
    Book Load();
    void Save(Book book);
}