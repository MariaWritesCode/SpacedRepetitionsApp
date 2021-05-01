using System.Collections.Generic;
using System.Threading.Tasks;
using SpacedRepApp.Share;

namespace SpacedRepApp.Infrastructure.Domain
{
    public interface ICategoryRepository
    {
        Task<IList<Category>> GetAll(bool includeAll = true);
        Task<Category> GetById(long id, bool IncludeAll = true);
        Task<Category> Create(Category category);
        Task<Category> Update(long id, Category category);
        Task Delete(long id);
    }
}