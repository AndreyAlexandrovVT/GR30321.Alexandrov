using GR30321.API.Controllers;
using GR30321.API.Data;
using GR30321.Domain.Entities;
using GR30321.Domain.Models;
using GR30321.Alexandrov.UI.Controllers;
using GR30321.Alexandrov.UI.Data;
using GR30321.Alexandrov.UI.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GR30321.Tests
{
    public class BooksControllerTests //: IDisposabl
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly IWebHostEnvironment _environment;
        public BooksControllerTests()
        {
            _environment = Substitute.For<IWebHostEnvironment>();
            // Create and open a connection. This creates the SQLite in-memory database, 
            //which will persist until the connection is closed
             // at the end of the test (see Dispose below).
             _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            // These options will be used by the context instances in this test suite, 
            //including the connection opened above.
             _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
             .UseSqlite(_connection)
             .Options;
            // Create the schema and seed some data
            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureCreated();
            var categories = new Category[]
            {
                    new Category {Name="", NormalizedName="fantastic"},
                    new Category {Name="", NormalizedName="horror"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
            var books = new List<Book>
            {
                 new Book {Name="", Description="", Price=0, Avtor = "", PublicationDate = 0, Image = "",
                 Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("fantastic"))},
                 new Book {Name = "", Description="", Price=0, Avtor = "", PublicationDate = 0, Image = "",
                 Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("fantastic"))},
                 new Book {Name = "", Description="", Price=0, Avtor = "", PublicationDate = 0, Image = "",
                 Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("horror"))},
                 new Book {Name = "", Description="", Price=0, Avtor = "", PublicationDate = 0, Image = "",
                 Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("horror"))},
                 new Book {Name = "", Description="", Price=0, Avtor = "", PublicationDate = 0, Image = "",
                 Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("horror"))}
                 };
            context.AddRange(books);
            context.SaveChanges();
        }
        public void Dispose() => _connection?.Dispose();
        AppDbContext CreateContext() => new AppDbContext(_contextOptions);
        // Проверка фильтра по категории
        [Fact]
        public async void ControllerFiltersCategory()
        {
            // arrange
            using var context = CreateContext();
            var category = context.Categories.First();
            var controller = new BooksController(context, _environment);
            // act
            var response = await controller.GetBooks(category.NormalizedName);
            ResponseData<ListModel<Book>> responseData = response.Value;
            var dishesList = responseData.Data.Items; // полученный список объектов
                                                      //assert
            Assert.True(dishesList.All(d => d.CategoryId == category.Id));
        }
        // Проверка подсчета количества страниц
        // Первый параметр - размер страницы
        // Второй параметр - ожидаемое количество страниц (при условии, что всего объектов 5)
            [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        public async void ControllerReturnsCorrectPagesCount(int size, int qty)
        {
            using var context = CreateContext();
            var controller = new BooksController(context, _environment);
            // act
            var response = await controller.GetBooks(null, 1, size);
            ResponseData<ListModel<Book>> responseData = response.Value;
            var totalPages = responseData.Data.TotalPages; // полученное количество страниц
             //assert
             Assert.Equal(qty, totalPages); // количество страниц совпадает
        }
        [Fact]
        public async void ControllerReturnsCorrectPage()
        {
            using var context = CreateContext();
            var controller = new BooksController(context, _environment);
            // При размере страницы 3 и общем количестве объектов 5
            // на 2-й странице должно быть 2 объекта
            int itemsInPage = 2;
            // Первый объект на второй странице
            Book firstItem = context.Books.ToArray()[3];
            // act
            // Получить данные 2-й страницы
            var response = await controller.GetBooks(null, 2);
            ResponseData<ListModel<Book>> responseData = response.Value;
            var dishesList = responseData.Data.Items; // полученный список объектов
            var currentPage = responseData.Data.CurrentPage; // полученный номер текущей страницы
                                                             //assert
            Assert.Equal(2, currentPage);// номер страницы совпадает
            Assert.Equal(2, dishesList.Count); // количество объектов на странице равно 2
            Assert.Equal(firstItem.Id, dishesList[0].Id); // 1-й объект в списке правильный
        }

        }
    }
