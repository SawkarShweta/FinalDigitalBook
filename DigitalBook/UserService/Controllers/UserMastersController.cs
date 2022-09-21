using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;


namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserMastersController : ControllerBase
    {
        private readonly DigitalBookContext _context;

        public UserMastersController(DigitalBookContext context)
        {
            _context = context;
        }

        // GET: api/UserMasters
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserMaster>>> GetUserMasters()
        {
            if (_context.UserMasters == null)
            {
                return NotFound();
            }
            var userMasters = await _context.UserMasters.ToListAsync();
            foreach(var user in userMasters)
            {
                user.UserPassword=EncryptionDecryptionController.Decrypt(user.UserPassword==null?"":user.UserPassword);
            }
            //return await _context.UserMasters.ToListAsync();
            return userMasters;
        }

        // GET: api/UserMasters/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserMaster>> GetUserMaster(int id)
        {
          if (_context.UserMasters == null)
          {
              return NotFound();
          }
            var userMaster = await _context.UserMasters.FindAsync(id);

            if (userMaster == null)
            {
                return NotFound();
            }

            return userMaster;
        }

        [HttpGet]
        [Route("GetUserMasterByName")]
        public async Task<ActionResult<UserMaster>> GetUserMasterByName(string name)
        {
            if (_context.UserMasters == null)
            {
                return NotFound();
            }
            var userMaster = await _context.UserMasters.Where(x=>x.UserName.Equals(name)).FirstOrDefaultAsync();

            if (userMaster == null)
            {
                return NotFound();
            }

            return userMaster;
        }

        // PUT: api/UserMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserMaster(int id, UserMaster userMaster)
        {
            if (id != userMaster.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userMaster).State = EntityState.Modified;

            try
            {
                userMaster.UserPassword = EncryptionDecryptionController.Encrypt(userMaster.UserPassword == null ? "" : userMaster.UserPassword);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMasterExists(id))
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

        // POST: api/UserMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserMaster>> PostUserMaster(UserMaster userMaster)
        {
            var isValidEmail = true;
            if (_context.UserMasters == null)
            {
                return Problem("Entity set 'DigitalBookContext.UserMasters'  is null.");
            }
            try
            {
                var isSameUserName= _context.UserMasters.Where(x => x.UserName == userMaster.UserName).Count();
                if (isSameUserName > 0)
                    return Problem("User Name already present");

                var countEmailId = _context.UserMasters.Where(x => x.EmailId == userMaster.EmailId && x.RoleId == userMaster.RoleId).Count();
                isValidEmail = countEmailId > 0 ? false : true;

                if (isValidEmail)
                {
                    userMaster.UserPassword = EncryptionDecryptionController.Encrypt(userMaster.UserPassword == null ? "" : userMaster.UserPassword);
                    _context.UserMasters.Add(userMaster);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetUserMaster", new { id = userMaster.UserId }, userMaster);
                }
                else
                {
                    return Problem("Email Id Already Present");
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }

        // DELETE: api/UserMasters/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserMaster(int id)
        {
            if (_context.UserMasters == null)
            {
                return NotFound();
            }
            var userMaster = await _context.UserMasters.FindAsync(id);
            if (userMaster == null)
            {
                return NotFound();
            }

            _context.UserMasters.Remove(userMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserMasterExists(int id)
        {
            return (_context.UserMasters?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }

}
