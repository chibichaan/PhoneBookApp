using System.Text.RegularExpressions;

namespace PhoneBook;

public class Book
{
    // * Функционал:
    // * Всё можно построчно редактировать.
    // * Добавить/удалить запись строчку
    
    private List<Row> _rows = new List<Row>();
    public List<Row> Rows
    {
        get { return _rows;}
        set { _rows = value; }
    }
    public void AddRow(Row row)
    {
        _rows.Add(row);
    }
    
    public void DeleteRow(Guid id)
    {
        var row = _rows.FirstOrDefault(r => r.Id == id);
        if (row != null)
        {
            _rows.Remove(row);
        }
    }

    public void EditRow(Guid choosenId, Row updatedRow)
    {
        var editingRow = _rows.FirstOrDefault(r => r.Id == choosenId);
        if (editingRow != null)
        {
            _rows.Remove(editingRow);
            _rows.Add(updatedRow);
        }
    }
}