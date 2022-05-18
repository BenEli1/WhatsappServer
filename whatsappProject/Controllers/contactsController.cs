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
    public class contactsController : ControllerBase
    {
        private readonly whatsappProjectContext _context;

        public contactsController(whatsappProjectContext context)
        {
            _context = context;
        }

        [HttpPut("Login")]
        public async Task<IActionResult> Login([Bind("username")] String username)
        {
            if (username == null)
            {
                return BadRequest();
            }

            HttpContext.Session.SetString("username", username);

            return NoContent();
        }

        // GET: api/contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetContact()
        {

            //string username = HttpContext.Session.GetString("username");
            string username = "sahar";
          if (_context.Contact == null)
          {
              return NotFound();
          }

            var list = _context.Contact
                        .Where(x => x.User.UserName == username)
                        .Select (x => new { x.id, x.name, x.server, x.last, x.lastdate }).ToArray();
           
            return list;
        }

        // GET: api/contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetContact(string id)
        {

            string username = HttpContext.Session.GetString("username");
            if (_context.Contact == null)
          {
              return NotFound();
          }

            var contact = _context.Contact
                        .Where(x => x.User.UserName == username && x.id == id)
                        .Select(x => new { x.id, x.name, x.server, x.last, x.lastdate }).ToArray();

            if (contact == null || contact.Length == 0)
            {
                return NotFound();
            }

            return contact[0];
        }

        // PUT: api/contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(string id, Contact contact)
        {

            string username = HttpContext.Session.GetString("username");

            if (id != contact.id && contact.User.UserName != username)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
           string username = HttpContext.Session.GetString("username");
            if (contact.User.UserName != username)
            {
                return BadRequest();
            }

            if (_context.Contact == null)
          {
              return Problem("Entity set 'whatsappProjectContext.Contact'  is null.");
          }
            _context.Contact.Add(contact);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ContactExists(contact.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetContact", new { id = contact.id }, contact);
        }

        // DELETE: api/contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            if (_context.Contact == null)
            {
                return NotFound();
            }

            string username = HttpContext.Session.GetString("username");

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null || contact.User.UserName != username)
            {
                return NotFound();
            }

            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(string id)
        {
            return (_context.Contact?.Any(e => e.id == id)).GetValueOrDefault();
        }

        // GET: api/contacts/alice/messages
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<Object>> GetContactMessages(string id)
        {

            //string username = HttpContext.Session.GetString("username");
            string username = "sahar";
            if (_context.Contact == null)
            {
                return NotFound();
            }

            var contact = _context.Contact
                        .Where(x => x.User.UserName == username && x.id == id)
                        .Select(x => new {x.messages}).ToArray();

            if (contact == null || contact.Length == 0)
            {
                return NotFound();
            }

            var mes = contact[0].messages.Where( x => true).Select(x => new { id = x.Id, content = x.Text, created = x.Date, sent = x.InOut });

            return mes.ToList();
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> PostMessage(string id, Message message)
        {
            var contact = await _context.Contact.FindAsync(id);
            string username = HttpContext.Session.GetString("username");

            if (contact == null || contact.User.UserName != username)
            {
                return BadRequest();
            }

            contact.messages.Add(message);

            return await PutContact(id, contact);
        }

        [HttpGet("{id}/messages/{id2}")]
        public async Task<ActionResult<Object>> GetSpecificMessage(string id, int id2)
        {

            //string username = HttpContext.Session.GetString("username");
            string username = "sahar";
            if (_context.Contact == null)
            {
                return NotFound();
            }

            var contact = _context.Contact
                        .Where(x => x.User.UserName == username && x.id == id)
                        .Select(x => new { x.messages }).ToArray();

            if (contact == null || contact.Length == 0)
            {
                return NotFound();
            }

            var mes = contact[0].messages.Where(x => x.Id == id2).Select(x => new { id = x.Id, content = x.Text, created = x.Date, sent = x.InOut });

            if (!mes.Any())
            {
                return NotFound();
            }

            return mes.ToList()[0];
        }

        [HttpDelete("{id}/messages/{id2}")]
        public async Task<IActionResult> DeleteSpecificMessage(string id, int id2)
        {

            var contact = await _context.Contact.FindAsync(id);
            string username = HttpContext.Session.GetString("username");

            if (contact == null || contact.User.UserName != username)
            {
                return BadRequest();
            }

            Message toDelete = contact.messages.Find(x => x.Id == id2);

            if (toDelete != null)
            {
                contact.messages.Remove(toDelete);
            }

            return await PutContact(id, contact);
        }

        [HttpPut("{id}/messages/{id2}")]
        public async Task<IActionResult> PutSpecificMessage(string id, int id2, Message message)
        {

            var contact = await _context.Contact.FindAsync(id);
            string username = HttpContext.Session.GetString("username");

            if (contact == null || contact.User.UserName != username)
            {
                return BadRequest();
            }

            Message toDelete = contact.messages.Find(x => x.Id == id2);

            if (toDelete != null)
            {
                contact.messages.Remove(toDelete);
                contact.messages.Add(message);
            }
            return await PutContact(id, contact);
        }
    }
}
