namespace PhoneBook;

public class Row
{
    //Каждая строчка хранит номер телефона, ФИО, город, улица, дом, кв, фотография, сфой идентификатор
    
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string FIO { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string Apartment { get; set; }
    public string PhotoLink { get; set; }

    public Row(Guid id, string phoneNumber, string fio, string city, string street, string house, string apartment) 
    {
        Id = id;
        PhoneNumber = phoneNumber;
        FIO = fio;
        City = city;
        Street = street;
        House = house;
        Apartment = apartment;
        // PhotoLink = photolink;
    }

    public Row()
    {
        
    }

}