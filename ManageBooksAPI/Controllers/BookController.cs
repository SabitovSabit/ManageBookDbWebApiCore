using ManageBooksAPI.AuthenticateModels;
using ManageBooksAPI.Interfaces;
using ManageBooksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageBooksAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    public class BookController : ControllerBase
    {
        private ICommonRepository<Book> _repository;
        private readonly BookDbContext _db;
        public BookController(BookDbContext db, ICommonRepository<Book> repository)
        {
            _repository = repository;
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _repository.GetAll();
            if (books != null)
            {
                return Ok(books);
            }
            return NotFound("Book table is empty!");
        }
        [HttpGet]

        public async Task<IActionResult> GetById(int id)
        {
            var book = await _repository.GetById(id);
            if (book != null)
            {
                return Ok(book);
            }
            return NotFound("This book didn't find");
        }
        [HttpGet]
        public async Task<IActionResult> GetByGenre(string name)
        {
            var _object = await _db.BookGenres.Where(x => x.Genre.Name == name).Select(x => x.Book).ToListAsync();
            if (_object != null)
            {
                return Ok(_object);
            }
            return NotFound("This book didn't find");
        }
        [HttpGet]
        public async Task<IActionResult> GetInfoBook(string name)
        {
            var bookinfo = await _db.AuthorBooks.Where(x => x.Book.Name == name)
                                               .Join(_db.BookGenres, ab => ab.BookId, bg => bg.BookId, (ab, bg) => new { ab, bg })
                                               .Select(y => new BookInfo { BookGenre = y.bg.Genre.Name, BookAuthor = y.ab.Author.Name })
                                               .ToListAsync();
            if (bookinfo != null)
            {
                return Ok(bookinfo);
            }
            return NotFound("This book didn't find");
        }
        [HttpGet]
        public async Task<IActionResult> GetTopGenre(int count)
        {

            var result = await _db.BookGenres.GroupBy(x => x.GenreId).OrderByDescending(x => x.Count())
                                                .Select(z => new { GenreId = z.Key }).Take(count)
                                                    .ToListAsync();
            //var result = await _db.Genres.Join(_db.BookGenres, g => g.Id, bg => bg.GenreId, (g, bg) => new { g, bg })
            //                              .GroupBy(x => x.bg.GenreId)
            //                              .Select(t =>t.Key)
            //                                .OrderByDescending(o => o.Count()).ToListAsync();

            if (result != null)
            {

                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> GetTopAuthor(int count)
        {

            var result = await _db.AuthorBooks.GroupBy(x => x.AuthorId).OrderByDescending(x => x.Count())
                                                .Select(z => new { AuthorId = z.Key }).Take(count)
                                                   .ToListAsync();

            if (result != null)
            {

                return Ok(result);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var newbook = await _repository.Create(book);
                    if (newbook != null)
                    {
                        transaction.Commit();
                        return Ok(newbook);

                    }
                    else
                    {
                        return Conflict("This book is already exist!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Book book)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (await _repository.Update(book) != null)
                    {
                        transaction.Commit();
                        return Ok(book);
                    }
                    else
                    {
                        return NotFound("Book is not exist!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (_repository.GetById(id) != null)
                    {
                        await _repository.Delete(id);
                        transaction.Commit();
                        return Ok();
                    }
                    else
                    {
                        return NotFound("Book is not exist!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
