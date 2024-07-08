namespace PhoneBook.Exceptions;

public class IdNotExistException : ApplicationException
{
    public IdNotExistException(string message) : base(message){}
}