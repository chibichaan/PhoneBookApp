namespace PhoneBook.Exceptions;

public class NumberNotExistException : ApplicationException
{
    public NumberNotExistException (string message) : base(message){}
}