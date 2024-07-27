using GR30321.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GR30321.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Uri проекта
            var uri = "https://localhost:7002/";
            
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций
            await context.Database.MigrateAsync();

            // Заполнение данными
            if (!context.Categories.Any() && !context.Books.Any())
            {
                var categories = new Category[]
                {
                     
                new Category {Name="Художественная литература",
                                NormalizedName="ImaginativeLiterature"},
                new Category {Name="Детская литература",
                                NormalizedName="ChildLiterature"},
                new Category {Name="Бизнес-литература",
                                NormalizedName="BusinessLiterature"},
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                var books = new List<Book>
                {
                   
                 new Book {Name ="Маркетинг", Avtor="Иван Иванов",
                            PublicationDate= 2019, Description ="Маркетинг в деталях",
                            Price = 13.56, Image=uri+"Images/1.jpg", Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("ImaginativeLiterature"))},

                new Book {Name ="Менеджмент", Avtor="Сергей Сергеев",
                            PublicationDate= 2017, Description ="Искуство управлять персоналом",
                            Price = 20.19, Image=uri+"Images/2.jpg", Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("ImaginativeLiterature"))},

                new Book {Name ="Экономика", Avtor="Петр Петров",
                            PublicationDate= 2022, Description ="Вопросы по микро и макроэкономике",
                            Price = 26.97, Image=uri + "Images/9.jpg", Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("BusinessLiterature"))},

                new Book {Name ="Бухучет", Avtor="Ирина Иринова",
                            PublicationDate= 2018, Description ="Секреты проводок и счетов",
                            Price = 5.69, Image=uri+"Images/4.jpg", Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("ChildLiterature"))},

                // new Book {Id = 1, Name ="Маркетинг", Avtor="Иван Иванов",
                //PublicationDate= 2005, Description ="Маркетинг в деталях. ",
                //Price = 13.56, Image="Images/1.jpg", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("ImaginativeLiterature")).Id, Category = _categories.Find(c => c.NormalizedName.Equals("ImaginativeLiterature")) },

                //new Book {Id = 2, Name ="Менеджмент", Avtor="Сергей Сергеев",
                //PublicationDate= 2006, Description ="Искуство управлять персоналом",
                //Price = 20.19, Image="Images/2", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("ImaginativeLiterature")).Id, Category = _categories.Find(c=>c.NormalizedName.Equals("ImaginativeLiterature"))},

                //new Book {Id = 3, Name ="Экономика", Avtor="Петр Петров",
                //PublicationDate= 2006, Description ="Вопросы по микро и макроэкономике",
                //Price = 26.97, Image="Images/3.jpg", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("BusinessLiterature")).Id, Category = _categories.Find(c=>c.NormalizedName.Equals("BusinessLiterature")) },

                //new Book {Id = 4, Name ="Бухучет", Avtor="Ирина Иринова",
                //PublicationDate= 2018, Description ="Секреты проводок и счетов",
                //Price = 5.65, Image="Images/4.jpg", CategoryId=_categories.Find(c=>c.NormalizedName.Equals("ChildLiterature")).Id, Category = _categories.Find(c => c.NormalizedName.Equals("ChildLiterature")) },



                };

                await context.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }

    }
}
