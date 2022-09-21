using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly DigitalBookContext _context;

        public PurchasesController(DigitalBookContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            return await _context.Purchases.ToListAsync();
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        {
            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            if (_context.Purchases == null)
            {
                return Problem("Entity set 'DigitalBookContext.Purchases'  is null.");
            }
            var purchaseCount = await _context.Purchases.Where(x => x.EmailId == purchase.EmailId && x.BookId == purchase.BookId).CountAsync();
            
            if (purchaseCount > 0)
            {
                return Problem("Book is already purchased by you.");
            }
            else
            {
                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);
            }
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseExists(int id)
        {
            return (_context.Purchases?.Any(e => e.PurchaseId == id)).GetValueOrDefault();
        }

        //[HttpGet]
        //[Route("GetBooksWithStatus")]
        //public async Task<ActionResult<IEnumerable<Purchase>>> GetBooksWithStatus(string emailId)
        //{
        //    if (_context.Purchases == null)
        //    {
        //        return NotFound();
        //    }
        //    var purchase = await _context.Purchases.Where(e => e.EmailId == emailId).ToListAsync();

        //    foreach (var p in purchase)
        //    {
        //        p.Book = await _context.Books.Where(x => x.BookId == p.BookId).SingleOrDefaultAsync();
        //        p.Book.Purchases.Clear();
        //    }

        //    return CreatedAtAction("GetPurchase", new { id = purchase.Select(x => x.PurchaseId) }, purchase);
        //}

        [HttpGet]
        [Route("GetBooksWithStatus")]
        public List<Purchase> GetBooksWithStatus(string EmailId)
        {
            List<Purchase> lsBookHistory = new List<Purchase>();
            if (_context.Purchases == null)
            {
                return lsBookHistory;
            }


            lsBookHistory = (from book in _context.Books
                             join user in _context.UserMasters
                             on book.UserId equals user.UserId
                             join category in _context.Categories
                             on book.CategoryId equals category.CategoryId
                             join purchase in _context.Purchases
                             on book.BookId equals purchase.BookId
                             into BookPurchaseGroup
                             from pur in BookPurchaseGroup.DefaultIfEmpty()
                             select new
                             {
                                 purchaseId = pur.PurchaseId == null ? 0 : pur.PurchaseId,
                                 bookId = book.BookId,
                                 Title = book.BookName,
                                 //Author = user.FirstName + " " + user.LastName,
                                 user=user,
                                 Publisher = book.Publisher,
                                 Price = book.Price,
                                 PublishedDate = book.PublishedDate,
                                 CategoryName = category.CategoryName,
                                 Email = pur.EmailId == null? "NA" : pur.EmailId,
                                 BookContent = book.Content,
                                 Active = book.Active
                             }).ToList()
            .Select(x => new Purchase()
            {
                BookId = x.bookId,
                //Title = x.Title,
                //Author = x.Author,
                //Publisher = x.Publisher,
                //Price = Convert.ToDouble(x.Price),
                //PublishedDate = x.PublishedDate,
                //CategoryName = x.CategoryName,
                EmailId = x.Email,
                //BookContent = x.BookContent,
                //Active = x.Active
                Book=new Book()
                {
                    BookId=x.bookId,
                    BookName=x.Title,
                    Price=x.Price,
                    Publisher=x.Publisher,
                    PublishedDate=x.PublishedDate,
                    Content=x.BookContent,
                    Active=x.Active,
                    User=x.user
                }
            }).ToList();

            lsBookHistory = lsBookHistory.Where(x => x.EmailId == EmailId || x.EmailId == "NA").ToList();
            //lsBookHistory = lsBookHistory.GroupBy(x => x.BookId).Select(y => y.First()).ToList();

            return lsBookHistory;
        }

        [HttpGet]
        [Route("GetBookHistory")]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetBookHistory(string emailId)
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.Where(e => e.EmailId == emailId).ToListAsync();

            foreach (var p in purchase)
            {
                p.Book = await _context.Books.Where(x => x.BookId == p.BookId).SingleOrDefaultAsync();
                p.Book.Purchases.Clear();
            }

            return CreatedAtAction("GetPurchase", new { id = purchase.Select(x => x.PurchaseId) }, purchase); ;
        }

        [HttpPost]
        [Route("PostPurchaseAzure")]
        public async Task<ActionResult<Purchase>> PostPurchaseAzure(Purchase purchase)
        {
            if (_context.Purchases == null)
            {
                return Problem("Entity set 'DigitalBookContext.Purchases'  is null.");
            }
            var purchaseCount = await _context.Purchases.Where(x => x.EmailId == purchase.EmailId && x.BookId == purchase.BookId).CountAsync();

            if (purchaseCount > 0)
            {
                return Problem("Book is already purchased by you.");
            }
            else
            {
                bool res = purchase.callPaymentAuzreFunPost();
                if (res)
                    return CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);
                else
                    return BadRequest();
            }
        }

    }
}
