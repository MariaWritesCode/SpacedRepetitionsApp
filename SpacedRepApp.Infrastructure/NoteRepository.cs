using SpacedRepApp.Infrastructure.Domain;
using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SpacedRepApp.Infrastructure
{
    public class NoteRepository : GenericRepository<Note>, INoteRepository
    {
        private new readonly SpacedRepAppDbContext _context;
        private readonly ITagRepository _tagRepository;

        public NoteRepository(SpacedRepAppDbContext context, ITagRepository tagRepository, ICacheService cacheService) : base(context, cacheService)
        {
            _context = context;
            _tagRepository = tagRepository;
        }
         
        public async Task<Note> Create(Note note)
        {
            note.Category = _context.Categories.Find(note.CategoryId);
            return await base.AddAsync(note);
        }

        public async Task<IList<Note>> GetAll(bool includeAll = true)
        {
            if (includeAll)
            {
                return
                    await _context.Notes.Include(x => x.Category)
                    .Include(x => x.Tags)
                    .ToListAsync();
            }
            else
            {
                return await _context.Notes.ToListAsync();
            }
        }

        public async Task<Note> GetById(long id, bool includeAll = true)
        {
            if(includeAll)
            {
                return await
                    _context.Notes.Include(x => x.Category)
                    .Include(x => x.Tags)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                return await _context.Notes.SingleOrDefaultAsync(x => x.Id == id);
            }
        }


        public async Task<List<Note>> GetNotesByTags(Tag tag)
        {

            return await _context.Notes
                .Where(x => x.Tags.Contains(tag))
                .ToListAsync();    
        }

public async Task<Note> Update(long id, Note updatedNote)
        {
            if (updatedNote == null)
            {
                return null;
            }

            Note exist = await
                    _context.Notes.Include(x => x.Category)
                    .Include(x => x.Tags)
                    .SingleOrDefaultAsync(x => x.Id == id);

            if (exist != null)
            { 
                _context.Entry(exist).State = EntityState.Detached;
                await FixTagsOnEdit(exist, updatedNote);                
                
             }

            return exist;
        }

        public async Task Delete(long id)
        {
            await base.DeleteAsync(id);
        }

        private async Task FixTagsOnEdit(Note noteToFix, Note newNote)
        {
            foreach(var tag in newNote.Tags)
            {
                if(!noteToFix.Tags.Any(x=> x.Name == tag.Name))
                {
                    await _tagRepository.Create(tag);
                }
            }
            _context.Entry(noteToFix).Entity.Tags.Clear();
            _context.Entry(noteToFix).Entity.Tags.AddRange(newNote.Tags);
            _context.Entry(noteToFix).CurrentValues.SetValues(newNote);
            _context.Entry(noteToFix).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
        }
    }
}
