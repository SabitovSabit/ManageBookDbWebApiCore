using ManageBooksAPI.Interfaces;
using ManageBooksAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageBooksAPI.Concrate
{
    public class BookRepository : ICommonRepository<Book>
    {
        private readonly BookDbContext _db;
        public BookRepository(BookDbContext db)
        {
            _db = db;
        }

        public async Task<List<Book>> GetAll()
        {
            var books= await _db.Books.ToListAsync();
            if (books != null)
            {
                return books;
            }
            return null;
        }

        public async Task<Book> GetById(int Id)
        {
            var _object = await _db.Books.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (_object != null)
            {
                return _object;
            }
            return null;
        }
       
       
        public async Task<Book> Create(Book _object)
        {
            bool ishave = _db.Books.Any(x => x.Name == _object.Name);
            if (!ishave)
            {
                _db.Books.Add(_object);
                await _db.SaveChangesAsync();
                return _object;
            }
            return null;
        }

        public async Task<Book> Update(Book _object)
        {
            var existingBook = await _db.Books.Where(s => s.Id == _object.Id).FirstOrDefaultAsync();

            if (existingBook != null)
            {
                existingBook.Name = _object.Name;

                await _db.SaveChangesAsync();
                return _object;
            }
            return null;
        }
        public async Task Delete(int Id)
        {
            var _object = await GetById(Id);
            _db.Books.Remove(_object);
            await _db.SaveChangesAsync();
        }
    }
}
