using SpacedRepApp.Infrastructure.Domain;
using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SpacedRepApp.Infrastructure
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private new readonly SpacedRepAppDbContext _context;        
        public TagRepository(SpacedRepAppDbContext context, ICacheService cacheService) : base(context, cacheService)
        {
            _context = context;
        }

        public async Task<Tag> Create(Tag tag)
        {
            Tag exists = await _context.Tags
                .FirstOrDefaultAsync(x => x.Name == tag.Name);

            if (exists == null)
            {
                await _cacheService.Set($"{cacheKey}:{tag.Name}", tag);
                return await base.AddAsync(tag);
            }
            else return exists;
        }

        public async Task AddAll(List<Tag> tags)
        {
            foreach(Tag tag in tags)
            {
                await Create(tag);
            }

            await _cacheService.Set(AllTagsKey, tags);
        }

        public async Task<List<Tag>> GetAll()
        {
            var allTags = await _context.Tags.ToListAsync();

            foreach(Tag tag in allTags)
            {
                if(await _cacheService.Get<Tag>($"{cacheKey}:{tag.Name}") == default)
                {
                    await _cacheService.Set($"{cacheKey}:{tag.Name}", tag);
                }
            }

            return allTags;
        }

        public async Task<Tag> GetByName(string tagName)
        {
            if(_cacheService.Get<Tag>($"{cacheKey}:{tagName}") == default)
            {                
                var value = await _context.Tags.FirstOrDefaultAsync (x => x.Name == tagName);
                await _cacheService.Set($"{cacheKey}:{tagName}", value);
                return value;
            }

            return await _cacheService.Get<Tag>($"{cacheKey}:{tagName}");
        }

        public async Task Delete(long id)
        {
            await base.DeleteAsync(id);
        }
    }
}
