namespace GR30321.Blazor.Services
{
    public interface IBookService<T> where T : class
    {
        event Action ListChanged; //должно сообщать о том, что  список объектов изменился
        
        // Список объектов
        IEnumerable<T> Books { get; }
        // Номер текущей страницы
        int CurrentPage { get; }
        // Общее количество страниц
        int TotalPages { get; }

        // Получение списка объектов
        Task GetBooks(int pageNo = 1, int pageSize = 3);
    }
}
