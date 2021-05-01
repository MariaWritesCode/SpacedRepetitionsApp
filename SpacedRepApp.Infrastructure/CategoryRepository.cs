using SpacedRepApp.Infrastructure.Domain;
using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SpacedRepApp.Infrastructure
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private new readonly SpacedRepAppDbContext _context;

        public CategoryRepository(SpacedRepAppDbContext context, ICacheService cacheService) : base(context, cacheService)
        {
            _context = context;
        }
        public async Task<Category> Create(Category category)
        {
            return await base.AddAsync(category);
        }

        public async Task<Category> GetById(long id, bool includeAll = true)
        {
            if(includeAll)
            {
                return await _context.Categories
                    .Include(x => x.Notes)
                    .ThenInclude(x => x.Tags)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                return await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<IList<Category>> GetAll(bool includeAll = true)
        {
            if (includeAll)
            {
               return await _context.Categories
                    .Include(x => x.Notes)
                    .ThenInclude(x => x.Tags)
                    .ToListAsync();
            }
            else
            {
                return await _context.Categories.ToListAsync();
            }
        }

        public async Task<Category> Update(long id, Category category)
        {
            return await base.UpdateAsync(category, id);
        }

        public async Task Delete(long id)
        {
            Category exists = await _context.Set<Category>().FindAsync(id);

            if (exists == null)
            {
                return;
            }

            _context.RemoveRange(_context.Notes.Where(x => x.CategoryId == id).ToList());
            _context.Remove(exists);

            await _context.SaveChangesAsync();
        }
    }
}
