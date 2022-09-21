using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DigitalBookContext _context;

        public BooksController(DigitalBookContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet]
        [Route("GetAllBooksByUserId")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksByUserId(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.Where(x=>x.UserId==id).ToListAsync();

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
          if (_context.Books == null)
          {
              return Problem("Entity set 'DigitalBookContext.Books'  is null.");
          }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }

        //Reader can search a book
        [HttpGet]
        [Route("searchBook")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBook(string title, string author, string publisher, DateTime releasedDate)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var authordata = await _context.UserMasters.Where(x => x.UserName.Equals(author)).ToListAsync();
            int userId = 0;
            if(authordata.Count() > 0)
                userId = authordata.Select(x=>x.UserId).FirstOrDefault();
            var book = await _context.Books.Where(x => x.BookName.Equals(title) || x.UserId == userId || x.Publisher == publisher || x.PublishedDate == releasedDate).ToListAsync();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                foreach(var b in book)
                {
                    if(b.User!=null)
                        b.User.Books = null;
                    else
                    {
                        b.User = await _context.UserMasters.Where(x => x.UserId == b.UserId).SingleOrDefaultAsync();
                        b.User.Books = null;
                    }
                }
            }
            return book;
        }


        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            if(_context.Categories == null)
            {
                return NotFound();
            }

            return await _context.Categories.ToListAsync();
        }

        [HttpPut]
        [Route("BlockUnblockBook")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Book>>> BlockUnblockBook(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            if (!BookExists(id))
            {
                return NotFound();
            }

            Book b = await _context.Books.Where(x => x.BookId == id).SingleOrDefaultAsync();
            if (b != null)
            {
                b.Active = !b.Active;
               _context.Entry(b).State= EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetBook", new { id = b.BookId }, b);
        }
    }
}
