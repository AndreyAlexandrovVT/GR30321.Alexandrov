using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using GR30321.API.Data;
using GR30321.Domain.Entities;
using GR30321.Alexandrov.UI.Services.BookService;
using GR30321.Alexandrov.UI.Services.CategoryService;

namespace GR30321.Alexandrov.UI.Areas.Admin.Pages
{
    public class EditModel(ICategoryService categoryService, IBookService bookService) : PageModel
    {
        //private readonly IBookService _bookService;

        //public EditModel(IBookService bookService)
        //{
        //    _bookService = bookService;
        //}

        [BindProperty]
        public Book Book { get; set; } = default!;
        public string? ErrorMessage { get; set; }
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await bookService.GetBookByIdAsync(id.Value);

            if (response == null)
            {
                return NotFound();
            }
            if (response.Success)
            {
                if (response.Data == null)
                {
                    return NotFound();
                }
                Book = response.Data;
            }
            else
            {
                ErrorMessage = response.ErrorMessage ?? "Unknown error.";
                return Page();
            }

            // Получим все категории и передим их во ViewData
            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
           "Name");

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Предполагаем, что объект Book передается с формы
            await bookService.UpdateBookAsync(id.Value,Book, Image);

            return RedirectToPage("./Index");
        }
    }
}
