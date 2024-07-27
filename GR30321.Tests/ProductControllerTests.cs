using GR30321.Domain.Entities;
using GR30321.Domain.Models;
using GR30321.Alexandrov.UI.Controllers;
using GR30321.Alexandrov.UI.Services.BookService;
using GR30321.Alexandrov.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR30321.Tests
{
    public class ProductControllerTests
    {
        IBookService _productService;
        ICategoryService _categoryService;
        public ProductControllerTests()
        {
            SetupData();
        }
        // Список категорий сохраняется во ViewData
        [Fact]
        public async void IndexPutsCategoriesToViewData()
        {
            //arrange
            var controller = new BookController(_categoryService, _productService);
            //act
            var response = await controller.Index(null);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            var categories = Assert.IsType<List<Category>>(view.ViewData["categories"]);
            Assert.Equal(6, categories.Count);
            Assert.Equal("Все", view.ViewData["currentCategory"]);
        }
        // Имя текущей категории сохраняется во ViewData
        [Fact]
        public async void IndexSetsCorrectCurrentCategory()
        {
            //arrange
            var categories = await _categoryService.GetCategoryListAsync();
            var currentCategory = categories.Data[0];
            var controller = new BookController(_categoryService, _productService);
            //act
            var response = await controller.Index(currentCategory.NormalizedName);
            //assert
            var view = Assert.IsType<ViewResult>(response);

            Assert.Equal(currentCategory.Name, view.ViewData["currentCategory"]);
        }
        // В случае ошибки возвращается NotFoundObjectResult
        [Fact]
        public async void IndexReturnsNotFound()
        {
            //arrange 
            string errorMessage = "Test error";
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Success = false;
            categoriesResponse.ErrorMessage = errorMessage;

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
            ;
            var controller = new BookController(_categoryService, _productService);
            //act
            var response = await controller.Index(null);
            //assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(errorMessage, result.Value.ToString());
        }
        // Настройка имитации ICategoryService и IProductService
        void SetupData()
        {
            _categoryService = Substitute.For<ICategoryService>();
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Data = new List<Category>
                 {
                 new Category {Id=1, Name="Фантастика", NormalizedName="fantastic"},
                 new Category {Id=2, Name="Ужасы", NormalizedName="horror"},
                 new Category {Id=3, Name="Историческая", NormalizedName="historical"},
                 new Category {Id=4, Name="Роман", NormalizedName="novel"},
                 new Category {Id=5, Name="Учебная", NormalizedName="educational"},
                 new Category {Id=6, Name="Научная", NormalizedName="scientific"}
                 };

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
            ;
            _productService = Substitute.For<IBookService>();
            var dishes = new List<Book>
                     {
                     new Book {Id = 1 },
                     new Book { Id = 2 },
                     new Book { Id = 3 },
                     new Book { Id = 4 },
                     new Book { Id = 5 }
                     };
            var productResponse = new ResponseData<ListModel<Book>>();
            productResponse.Data = new ListModel<Book> { Items = dishes };
            _productService.GetBookListAsync(Arg.Any<string?>(), Arg.Any<int>())
            .Returns(productResponse);
        }

    }

}
