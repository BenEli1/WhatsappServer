using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Data;
using whatsappProject.Models;

namespace whatsappProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTokensController : ControllerBase
    {
        private readonly whatsappProjectContext _context;

        public UserTokensController(whatsappProjectContext context)
        {
            _context = context;
        }

        // GET: api/UserTokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserToken>>> GetUserToken()
        {
          if (_context.UserToken == null)
          {
              return NotFound();
          }
            return await _context.UserToken.ToListAsync();
        }

        // GET: api/UserTokens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserToken>> GetUserToken(string id)
        {
          if (_context.UserToken == null)
          {
              return NotFound();
          }
            var userToken = await _context.UserToken.FindAsync(id);

            if (userToken == null)
            {
                return NotFound();
            }

            return userToken;
        }

        // POST: api/UserTokens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserToken>> PostUserToken([FromBody]UserToken userToken)
        {
          if (_context.UserToken == null)
          {
              return Problem("Entity set 'whatsappProjectContext.UserToken'  is null.");
          }
            _context.UserToken.Add(userToken);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTokenExists(userToken.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserToken", new { id = userToken.UserName }, userToken);
        }

        private bool UserTokenExists(string id)
        {
            return (_context.UserToken?.Any(e => e.UserName == id)).GetValueOrDefault();
        }
    }
}
