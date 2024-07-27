using GR30321.Domain.Entities;
using GR30321.Domain.Models;
using GR30321.Alexandrov.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace GR30321.Alexandrov.UI.Services.BookService
{
    public class MemoryBookService : IBookService
    {
        List<Book> _books;
        List<Category> _categories;
       private readonly IConfiguration _config;

        public MemoryBookService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync()
           .Result
           .Data;
           _config = config;
            SetupData();
        }

        // Инициализация списков
        private void SetupData()
        {
            _books = new List<Book>
            {
                new Book {Id = 1, Name ="Маркетинг", Avtor="Иван Иванов",
                PublicationDate= 2005, Description ="Маркетинг в деталях. ",
                Price = 13.56, Image="Images/1.jpg", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("ImaginativeLiterature")).Id, Category = _categories.Find(c => c.NormalizedName.Equals("ImaginativeLiterature")) },

                new Book {Id = 2, Name ="Менеджмент", Avtor="Сергей Сергеев",
                PublicationDate= 2006, Description ="Искуство управлять персоналом",
                Price = 20.19, Image="Images/2", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("ImaginativeLiterature")).Id, Category = _categories.Find(c=>c.NormalizedName.Equals("ImaginativeLiterature"))},

                new Book {Id = 3, Name ="Экономика", Avtor="Петр Петров",
                PublicationDate= 2006, Description ="Вопросы по микро и макроэкономике",
                Price = 26.97, Image="Images/3.jpg", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("BusinessLiterature")).Id, Category = _categories.Find(c=>c.NormalizedName.Equals("BusinessLiterature")) },

                new Book {Id = 4, Name ="Бухучет", Avtor="Ирина Иринова",
                PublicationDate= 2018, Description ="Секреты проводок и счетов",
                Price = 5.69, Image="Images/4.jpg", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("ChildLiterature")).Id, Category = _categories.Find(c => c.NormalizedName.Equals("ChildLiterature")) },

            };
        }
        public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            //var model = new ListModel<Book>() { Items = _books };
            //var result = new ResponseData<ListModel<Book>>()

            //{ Data = model };
            //return Task.FromResult(result);

            // Создать объект результата
            var result = new ResponseData<ListModel<Book>>();
            // Id категории для фильрации
            int? categoryId = null;
            // если требуется фильтрация, то найти Id категории с заданным categoryNormalizedName
            if (categoryNormalizedName != null)
                categoryId = _categories.Find(c =>c.NormalizedName.Equals(categoryNormalizedName))?.Id;
            // Выбрать объекты, отфильтрованные по Id категории, если этот Id имеется
            var data = _books
            .Where(d => categoryId == null ||
           d.CategoryId.Equals(categoryId))?
            .ToList();

            // получить размер страницы из конфигурации
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();
            // получить общее количество страниц
            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);
            // получить данные страницы
            var listData = new ListModel<Book>()
            {
                Items = data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };


            // поместить ранные в объект результата
            //result.Data = new ListModel<Book>() { Items = data };
           result.Data = listData;

            // Если список пустой
            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбраннной категории";
            }
            // Вернуть результат
            return Task.FromResult(result);

        }

        public Task<ResponseData<Book>> UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
