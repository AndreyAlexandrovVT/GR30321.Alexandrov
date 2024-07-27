using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
//using GR30321.API.Data;
using GR30321.Domain.Entities;
using GR30321.Alexandrov.UI.Services.BookService;
using GR30321.Alexandrov.UI.Services.CategoryService;

namespace GR30321.Alexandrov.UI.Areas.Admin.Pages
{
    public class CreateModel(ICategoryService categoryService, IBookService productService) : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
           "Name");
            return Page();
        }
        [BindProperty]
        public Book Book { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }
       
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await productService.CreateBookAsync(Book, Image);
            return RedirectToPage("./Index");
        }

        }

    }
