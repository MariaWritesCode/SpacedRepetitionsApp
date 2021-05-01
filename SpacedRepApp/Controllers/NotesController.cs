using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpacedRepApp.Infrastructure.Domain;
using SpacedRepApp.Share;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpacedRepApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;

        public NotesController(INoteRepository noteRepository, ITagRepository tagRepository)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
        }

        // GET: api/<NotesController>
        [HttpGet]
        public async Task<ActionResult<IList<Note>>> GetAll(bool includeAll = true)
        {
            return Ok(await _noteRepository.GetAll(includeAll));
        }

        // GET: api/<NotesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(long id)
        {
            return Ok(await _noteRepository.GetById(id));
        }

        // POST api/<NotesController>
        [HttpPost]
        public async Task<ActionResult<Note>> Post(NoteDto item)
        {
            var note = new Note {
                Id = item.Id,
                DateCreated = DateTime.Now,
                Contents = item.Contents,
                Revised = false,
                CategoryId = item.CategoryId,
                Tags = item.Tags
             };

            await _tagRepository.AddAll(note.Tags);
            var created = await _noteRepository.Create(note);

            return CreatedAtAction(nameof(this.Get), new { id = created.Id }, created);
        }

        // PUT api/<NotesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, NoteDto item)
        {          

            var updatedNote = new Note {
                Id = item.Id,
                DateCreated = item.DateCreated,
                Contents = item.Contents,
                Revised = item.Revised,
                CategoryId = item.CategoryId,
                Tags = item.Tags
             };

            await _tagRepository.AddAll(updatedNote.Tags);
            
            var updated = await _noteRepository.Update(id, updatedNote);

            if(updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/<NotesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _noteRepository.Delete(id);
        }
    }
}
