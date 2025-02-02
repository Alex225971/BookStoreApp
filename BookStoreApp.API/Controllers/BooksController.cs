﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Models.Book;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using BookStoreApp.API.Repositories;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(IBooksRepository booksRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _booksRepo = booksRepo;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            var books = await _booksRepo.GetAllReadOnlyAsync();
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            var book = await _booksRepo.GetDetailsAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        private string CreateFile(string imgBase64, string imgName)
        {
            var url = HttpContext.Request.Host.Value;
            var ext = Path.GetExtension(imgName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var path = $"{_webHostEnvironment.WebRootPath}\\res\\books\\img\\{fileName}";

            byte[] image = Convert.FromBase64String(imgBase64);

            var fileStream = System.IO.File.Create(path);
            fileStream.Write(image, 0, image.Length);
            fileStream.Close();

            return $"https://{url}/res/books/img/{fileName}";
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }
            var book = await _booksRepo.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(bookDto.ImageData))
            {
                bookDto.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);

                var imgName = Path.GetFileName(book.Image);
                var path = $"{_webHostEnvironment.WebRootPath}\\res\\books\\img{imgName}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }

            _mapper.Map(bookDto, book);

            try
            {
                await _booksRepo.UpdateAsync(book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            if (string.IsNullOrEmpty(bookDto.ImageData) == false)
            {
                book.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);
            }

            await _booksRepo.AddAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _booksRepo.GetAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _booksRepo.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await _booksRepo.Exists(id);
        }
    }
}
