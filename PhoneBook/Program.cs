using PhoneBook;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using PhoneBook.Exceptions;

/*
 * Книга состоит из строчек. Каждая строчка - отдельный объект книги.
 * Каждая строчка хранит номер телефона, ФИО, город, улица, дом, кв, фотография
 *
 * Функционал:
 * Всё можно построчно редактировать.
 * Добавить/удалить запись строчку
 *
*/
namespace PhoneBook
{
 public class Utf8StringWriter : StringWriter
 {
  public override Encoding Encoding => Encoding.UTF8;
 }
 class Program
 {
  private static Book _phoneBook;
  private static IFileManager _fileManager;
  public static int inputNumberOfFormat = 0;
  
  static void Main(string[] args)
  {
   bool showMenu = true;
   
   Console.WriteLine("Выберите 1, если хотите читать из формата XML, 2 - из формата Json");
   
   switch (Console.ReadLine())
   {
    case "1":
     _fileManager = new XMLManager();
     break;
    case "2":
     _fileManager = new JsonFileManager();
     break;
    default:
     Console.WriteLine("Неккоректный ввод. Проверьте вводимое значение и попробуйте снова!");
     break;
   }
   
   _phoneBook = _fileManager.Load();
   while (showMenu)
   {
    showMenu = MainMenu(_phoneBook);
   }
  }
  
 static bool MainMenu(Book book)
 {
  Console.WriteLine("Что вы хотите сделать?");
  Console.WriteLine("1 - Добавить строку в телефонную книгу");
  Console.WriteLine("2 - Удалить строку в телефонной книге");
  Console.WriteLine("3 - Редактировать строку в телефонной книге");
  Console.WriteLine("4 - Вывести список в телефонной книге");
  Console.WriteLine("5 - Найти контакт");
  Console.WriteLine("0 - Выйти из телефонной книги");
  

  switch (Console.ReadLine())
  {
   case "1":
    Console.Clear();
    try
    {
     ToAddNewRow(book);
    }
    catch (IncorrectInputException e)
    {
     Console.WriteLine(e);
    }

    return true;

   case "2":
    Console.Clear();
    try
    {
     ToDeleteRow(book);
    }
    catch (IncorrectInputException e)
    {
     Console.WriteLine(e);
    }
    catch (NumberNotExistException e)
    {
     Console.WriteLine(e);
    }

    return true;

   case "3":
    Console.Clear();
    try
    {
     ToEditRow(book);
    }
    catch (IncorrectInputException e)
    {
     Console.WriteLine(e);
    }
    catch (NumberNotExistException e)
    {
     Console.WriteLine(e);
    }

    return true;

   case "4":
    Console.Clear();
    Console.WriteLine("Список добавленных номеров:");
    foreach (var r in book.Rows)
    {
     Console.WriteLine($"\nId: {r.Id};" +
                       $"\nНомер: {r.PhoneNumber};" +
                       $"\nФИО: {r.FIO}" +
                       $"\nГород: {r.City}" +
                       $"\nУлица: {r.Street}" +
                       $"\nДом: {r.House}" +
                       $"\nКвартира: {r.Apartment} " +
                       $"\n***************************");
    }
    return true;
   case "5":
    Console.Clear();
    var inputString = GetInputLine("Введите номер или ФИО, которое хотите найти");
    var selectedRows = book.Rows.Where(r => 
    { 
     return r.PhoneNumber.Contains(inputString) 
            || r.FIO.Contains(inputString);
    });
    
    foreach (var row in selectedRows)
    {
     Console.WriteLine($"\nId: {row.Id};\nНомер: {row.PhoneNumber};\nФИО: {row.FIO}");
    }
    return true;
   case "0":
    Console.Clear();
    Console.WriteLine("Благодарим, что воспользовались нашей телефонной книгой!");
    return false;
   default:
    Console.Clear();
    Console.WriteLine("Неккоректный ввод. Проверьте вводимое значение и попробуйте снова!");
    return true;
  }
 }

 /// <summary>
 /// Добавление строчки в книгу
 /// </summary>
 /// <param name="book"></param>
 static void ToAddNewRow(Book book)
 {
  
  var row = GetInputRowData(Guid.NewGuid());

   book.AddRow(row);
   _fileManager.Save(book);
   
   Console.WriteLine("В телефонную книгу добавился номер:");
   Console.WriteLine($"\nId: {row.Id};" +
                     $"\nНомер: {row.PhoneNumber};" +
                     $"\nФИО: {row.FIO}" +
                     $"\nГород: {row.City}" +
                     $"\nУлица: {row.Street}" +
                     $"\nДом: {row.House}" +
                     $"\nКвартира: {row.Apartment}");
  }

 /// <summary>
 /// Удаление строчки из книги
 /// </summary>
 /// <param name="book"></param>
 /// <exception cref="IdNotExistException"></exception>
  static void ToDeleteRow(Book book)
  {
   Console.WriteLine("Номера, доступные для удаления:");
   foreach (var r in book.Rows)
   {
    Console.WriteLine($"\nId: {r.Id};\nНомер: {r.PhoneNumber};\nФИО: {r.FIO}");
   }

   var inp = Guid.Empty;
   if (Guid.TryParse(GetInputLine("Введите id контакта, который хотите удалить"), out Guid result))
   {
     inp = result;
   }

   var choosenDeleteRow = book.Rows.FirstOrDefault(c => c.Id == inp);
   if (choosenDeleteRow.Id != inp)
   {
    throw new IdNotExistException("Данного id не существует в телефонной книге");
   }

   book.DeleteRow(choosenDeleteRow.Id);
   _fileManager.Save(book);

   Console.WriteLine("Теперь в телефонной книге сохранены номера:");
   foreach (var r in book.Rows)
   {
    Console.WriteLine($"\nId: {r.Id};\nНомер: {r.PhoneNumber};\nФИО: {r.FIO}");
   }
  }

 /// <summary>
 /// Редактирование строчки
 /// </summary>
 /// <param name="book"></param>
 /// <exception cref="NumberNotExistException">Ошибка существования номера</exception>
  static void ToEditRow(Book book)
  {
   Console.WriteLine("Номера в телефонной книге, доступные для редактирования:");
   foreach (var r in book.Rows)
   {
    Console.WriteLine($"\nId: {r.Id};\nНомер: {r.PhoneNumber};\nФИО: {r.FIO}");
   }

   var choosenIdForEdit = Guid.Parse(GetInputLine("Введите id контакта, который хотите поменять"));

   var choosenEditRow = book.Rows.FirstOrDefault(c => c.Id == choosenIdForEdit);
   if (choosenEditRow.Id != choosenIdForEdit)
   {
    throw new NumberNotExistException("Данного номера не существует в телефонной книге");
   }

   var row = GetInputRowData(choosenIdForEdit);
   
   book.EditRow(choosenIdForEdit, row);
   _fileManager.Save(book);
   
   foreach (var r in book.Rows)
   {
    Console.WriteLine($"\nId: {r.Id};" +
                      $"\nНомер: {r.PhoneNumber};" +
                      $"\nФИО: {r.FIO}" +
                      $"\nГород: {r.City}" +
                      $"\nУлица: {r.Street}" +
                      $"\nДом: {r.House}" +
                      $"\nКвартира: {r.Apartment}");
   }

  }

 /// <summary>
 /// Проверка номера теелефона
 /// </summary>
 /// <param name="input"></param>
 /// <exception cref="IncorrectInputException">Неверное введенное значение</exception>
  static void CheckPhoneNumber(string input)
  {
   //проверка номера путем применения регулярных выражений.
   var correntNumber = new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");

   if (!correntNumber.IsMatch(input))
   {
    throw new IncorrectInputException("Данный номер записан неверно!");
   }
   // коректрый ввод:
   // +79261234567
   // 89261234567
   //8(926)123-45-67
   // 123-45-67
   //8-926-123-45-67
   //8 927 1234 234

  }

  /// <summary>
  /// Метод быстрого вывода сообщения для пользователя
  /// </summary>
  /// <param name="message">сообщение, которое нужно вывести</param>
  /// <returns></returns>
  static string? GetInputLine(string message)
  {
   Console.WriteLine(message);
   return Console.ReadLine();
  }

  /// <summary>
  /// Метод получения всей вводимой информации, добавленной в строчку
  /// </summary>
  /// <param name="id">айди контакта</param>
  /// <returns></returns>
  static Row GetInputRowData(Guid id)
  {
   var inputNumber = GetInputLine("Введите новый номер телефона")?.Trim();
   var inputFIO = GetInputLine("Введите новое ФИО")?.Trim();
   var inputCity = GetInputLine("Введите город")?.Trim();
   var inputStreet = GetInputLine("Введите улицу")?.Trim();
   var inputHouse = GetInputLine("Введите дом")?.Trim();
   var inputApartment = GetInputLine("Введите номер квартиры")?.Trim();

   CheckPhoneNumber(inputNumber);
   
   var row = new Row
   {
    Id = id,
    PhoneNumber = inputNumber,
    FIO = inputFIO,
    City = inputCity,
    Street = inputStreet,
    House = inputHouse,
    Apartment = inputApartment
   };

   return row;
  }
 }
}
  



