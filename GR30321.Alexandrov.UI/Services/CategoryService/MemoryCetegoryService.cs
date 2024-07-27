using GR30321.Domain.Entities;
using GR30321.Domain.Models;

namespace GR30321.Alexandrov.UI.Services.CategoryService
{
    public class MemoryCetegoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Художественная литература",
                                NormalizedName="ImaginativeLiterature"},
                new Category {Id=2, Name="Детская литература",
                                NormalizedName="ChildLiterature"},
                new Category {Id=3, Name="Бизнес-литература",
                                NormalizedName="BusinessLiterature"},

            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
