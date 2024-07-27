using GR30321.Domain.Entities;
using GR30321.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GR30321.Alexandrov.UI.Services.BookService
{
    public class ApiBookService(HttpClient httpClient) : IBookService
    {
        public async Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            // Подготовить объект, возвращаемый методом
            var responseData = new ResponseData<Book>();
            // Послать запрос к API для сохранения объекта
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, book);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось создать объект: {response.StatusCode}";
                return responseData;
            }
            // Если файл изображения передан клиентом
            if (formFile != null)
            {
                // получить созданный объект из ответа Api-сервиса
                var _book = await response.Content.ReadFromJsonAsync<Book>();
                // создать объект запроса
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{_book.Id}")
                };
                // Создать контент типа multipart form-data
                var content = new MultipartFormDataContent();
                // создать потоковый контент из переданного файла
                var streamContent = new StreamContent(formFile.OpenReadStream());
                // добавить потоковый контент в общий контент по именем "image"
                content.Add(streamContent, "image", formFile.FileName);
                // поместить контент в запрос
                request.Content = content;
                // послать запрос к Api-сервису
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось сохранить изображение: { response.StatusCode}";
                }
            }
            return responseData;

        }

        public async Task<ResponseData<Book>> DeleteBookAsync(int id)
        {
            var responseData = new ResponseData<Book>();
            // Отправляем DELETE запрос к API сервису
            var response = await httpClient.DeleteAsync($"{httpClient.BaseAddress}{id}");
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось удалить книгу: {response.StatusCode}";
                return responseData;
            }
            responseData.Success = true;
            return responseData;
        }

        public async Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {

            //Создадим URI для API запроса с указанием ID книги
            var uri = httpClient.BaseAddress + $"{id}";
            var result = await httpClient.GetAsync(uri);

            if (result.IsSuccessStatusCode)
            {
                // Десериализуем JSON ответ в объект ResponseData<Book>
                return await result.Content.ReadFromJsonAsync<ResponseData<Book>>();
            }
            // В случае ошибки возвращаем ResponseData с указанием ошибки
            var response = new ResponseData<Book>
            {
                Success = false,
                ErrorMessage = "Ошибка чтения API"
            };
            return response;
        }

        public async Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var uri = httpClient.BaseAddress;
            var queryData = new Dictionary<string, string>();
            queryData.Add("pageNo", pageNo.ToString());
            if (!String.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName);
            }
            var query = QueryString.Create(queryData);
            var result = await httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Book>>>();
            };
            var response = new ResponseData<ListModel<Book>>
            { Success = false, ErrorMessage = "Ошибка чтения API" };
            return response;
        }

        
        public async Task<ResponseData<Book>> UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var responseData = new ResponseData<Book>();
            var response = await httpClient.PutAsJsonAsync($"{httpClient.BaseAddress}{id}", book);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось обновить объект: {response.StatusCode}";
                return responseData;
            }
            if (formFile != null)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress}{id}/upload-image")
                };

                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(formFile.OpenReadStream());
                content.Add(streamContent, "image", formFile.FileName);
                request.Content = content;
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось сохранить изображение: {response.StatusCode}";
                }
            }
            responseData.Success = true;
            responseData.Data = book;
            return responseData;
        }
    }

}
