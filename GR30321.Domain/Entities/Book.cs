using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GR30321.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; } //наименование книги
        public string Description { get; set; } //описание книги
        public string? Image { get; set; } //картинка

        public string Avtor { get; set; } //автор книги
      
        public int PublicationDate { get; set; } //год издания
        public double Price { get; set; } //цена



        //навигационные поля
        public int CategoryId { get; set; }
        //[JsonIgnore] //игнорирование при сериализации
        public Category? Category { get; set; }

    }
}
