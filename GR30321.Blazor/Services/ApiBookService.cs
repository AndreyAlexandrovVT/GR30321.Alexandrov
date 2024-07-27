using GR30321.Domain.Entities;
using GR30321.Domain.Models;

namespace GR30321.Blazor.Services
{
    public class ApiBookService (HttpClient Http) : IBookService<Book>
    {
        List<Book> _books;
        int _currentPage = 1;
        int _totalPages = 1;
        public IEnumerable<Book> Books => _books;
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;
        public event Action ListChanged;
        public async Task GetBooks(int pageNo, int pageSize)
        {
            // Url сервиса API
            var uri = Http.BaseAddress.AbsoluteUri;
            // данные для Query запроса
            var queryData = new Dictionary<string, string>
                    {
                            { "pageNo", pageNo.ToString() },
                            {"pageSize", pageSize.ToString() }
                    };
            var query = QueryString.Create(queryData);
            // Отправить запрос http
            var result = await Http.GetAsync(uri + query.Value);
            // В случае успешного ответа
            if (result.IsSuccessStatusCode)
            {
                // получить данные из ответа
                var responseData = await result.Content
                .ReadFromJsonAsync<ResponseData<ListModel<Book>>>();
                // обновить параметры
                _currentPage = responseData.Data.CurrentPage;
                _totalPages = responseData.Data.TotalPages;
                _books = responseData.Data.Items;
                ListChanged?.Invoke();
            }
            // В случае ошибки
            else
            {
                _books = null;
                _currentPage = 1;
                _totalPages = 1;
            }
        }


    }
}