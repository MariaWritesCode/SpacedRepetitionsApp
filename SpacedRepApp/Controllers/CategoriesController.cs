using Microsoft.AspNetCore.Mvc;
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
    public class CategoriesController : ControllerBase
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IList<Category>>> GetAll(bool includeAll = true)
        {
            return Ok(await _categoryRepository.GetAll(includeAll));
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(long id)
        {
            return Ok(await _categoryRepository.GetById(id));
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] CategoryDto item)
        {
            var category = new Category {
            Id = item.Id,
            Name = item.Name
            };

            var created = await _categoryRepository.Create(category);

            return CreatedAtAction(nameof(this.Get), new { id = created.Id }, created);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] CategoryDto item)
        {
            var updatedCategory = new Category { 
                Id = item.Id,
                Name = item.Name
            };

            var updated = await _categoryRepository.Update(id, updatedCategory);

            if(updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _categoryRepository.Delete(id);
        }
    }
}
