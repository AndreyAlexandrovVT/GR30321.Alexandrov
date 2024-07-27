using GR30321.Domain.Entities;
using GR30321.Domain.Models;

namespace GR30321.Alexandrov.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        // Получение списка всех категорий
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
