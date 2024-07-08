namespace PhoneBook.Exceptions;

public class IncorrectInputException : ApplicationException
{
    public IncorrectInputException (string message) : base(message){}
}