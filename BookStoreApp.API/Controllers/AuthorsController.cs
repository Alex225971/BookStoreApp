using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Repositories;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsRespository _authorsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorsRespository authorsRepo, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _authorsRepo = authorsRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            try
            {
                var authors = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(await _authorsRepo.GetAllAsync());
                return Ok(authors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error performing GET operation in {nameof(GetAuthors)}");
                return StatusCode(500, "There was an error completing the request, check server connection and try again");
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id)
        {
            try
            {
                var author = await _authorsRepo.GetAuthorDetailsAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"Record not found {nameof(GetAuthor)} - ID {id}");
                    return NotFound();
                }

                return Ok(author);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error Performing GET in {nameof(GetAuthor)}");
                throw;
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto author)
        {
            if (id != author.Id)
            {
                _logger.LogWarning($"Update ID invalid in {nameof(PutAuthor)}: {id}");
                return BadRequest();
            }
            var authorFromDb = await _authorsRepo.GetAsync(id);

            if (authorFromDb == null)
            {
                _logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)}: {id}");
                return NotFound();
            }

            _mapper.Map(author, authorFromDb);

            try
            {
                await _authorsRepo.UpdateAsync(authorFromDb);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Error performing PUT in{nameof(PutAuthor)}");
                    return StatusCode(500, "Internal server error");
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                var author = _mapper.Map<Author>(authorDto);
                await _authorsRepo.AddAsync(author);

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error performing POST in{nameof(PostAuthor)}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _authorsRepo.GetAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)}: {id}");
                    return NotFound();
                }

                await _authorsRepo.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {

                _logger.LogError(e, $"Error performing DELETE in{nameof(DeleteAuthor)}");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _authorsRepo.Exists(id);
        }
    }
}
