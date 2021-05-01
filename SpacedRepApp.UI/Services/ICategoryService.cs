using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedRepApp.UI.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories(bool includeAll = true);
        Task<Category> GetCategory(long id, bool includeAll = true);
        Task AddCategory(Category category);
        Task DeleteCategory(long categoryId);
    }
}
