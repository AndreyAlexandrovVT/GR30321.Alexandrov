using GR30321.Domain.Entities;
using GR30321.Domain.Models;

namespace GR30321.Alexandrov.UI.Services.BookService
{
    public interface IBookService
    {

        /// Получение списка всех объектов 
        public Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? categoryNormalizedName, int pageNo = 1);

        /// Поиск объекта по Id   
        public Task<ResponseData<Book>> GetBookByIdAsync(int id);

        /// Обновление объекта  
        public Task<ResponseData<Book>> UpdateBookAsync(int id, Book book, IFormFile? formFile);

        /// Удаление объекта  
        public Task<ResponseData<Book>> DeleteBookAsync(int id);

        /// Создание объекта
        public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile);
    }
}
