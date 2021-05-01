using Microsoft.AspNetCore.Mvc;
using SpacedRepApp.DataDto;
using SpacedRepApp.Infrastructure.Domain;
using SpacedRepApp.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpacedRepApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private ITagRepository _tagRepository;

        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tag>>> Get()
            {
            return Ok(await _tagRepository.GetAll());
        }

        // POST api/<TagController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TagDto newTag)
        {
            var tag = new Tag
            {
                Id = newTag.Id,
                Name = newTag.Name,
            };

            var created = await _tagRepository.Create(tag);
            return CreatedAtAction(nameof(this.Get), new { id = created.Id }, created);
        }

        // PUT api/<TagController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TagController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
