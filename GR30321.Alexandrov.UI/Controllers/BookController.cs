using GR30321.Domain.Entities;
using GR30321.Alexandrov.UI.Services.BookService;
using GR30321.Alexandrov.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace GR30321.Alexandrov.UI.Controllers
{
    public class BookController(ICategoryService categoryService, IBookService bookService) : Controller
    {
        [Route("Catalog")]
        [Route("Catalog/{category}")]
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            // получить список категорий
            var categoriesResponse = await categoryService.GetCategoryListAsync();
            // если список не получен, вернуть код 404
            if (!categoriesResponse.Success)
                return NotFound(categoriesResponse.ErrorMessage);
            // передать список категорий во ViewData 
            ViewData["categories"] = categoriesResponse.Data;
            // передать во ViewData имя текущей категории
            var currentCategory = category == null? "Все": categoriesResponse.Data.FirstOrDefault(c =>c.NormalizedName == category)?.Name;
            ViewData["currentCategory"] = currentCategory;


            var bookResponse = await bookService.GetBookListAsync(category, pageNo);
            if (!bookResponse.Success)
                ViewData["Error"] = bookResponse.ErrorMessage;
            //return View(bookResponse.Data.Items);
            return View(bookResponse.Data);
        }
    }
}
