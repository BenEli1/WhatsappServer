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

        // GET: api/contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetContact(string username)
        {

          if (_context.Contact == null)
          {
              return new List<Object>();
          }

            var contacts = await _context.Contact.ToArrayAsync();
            var List = from c in contacts
                       where c.UserName == username 
                       select new { c.id, c.name, c.server, c.last, c.lastdate };

            return List.ToArray();
        }

        // GET: api/contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetContact(string id, string username)
        {
            if (_context.User == null || _context.User.FindAsync(username) == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contacts = await _context.Contact.ToArrayAsync();
            var List = from c in contacts
                       where c.UserName == username && c.id == id
                       select new { c.id, c.name, c.server, c.last, c.lastdate };

            var contact = List.FirstOrDefault();

            return contact;
        }

        // PUT: api/contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(string id, Contact contact, string username)
        {
            if (id != contact.id && username != contact.UserName)
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
        public async Task<ActionResult<Contact>> PostContact([FromBody] Contact contact, string username)
        {
          if (_context.Contact == null)
          {
              return Problem("Entity set 'whatsappProjectContext.Contact'  is null.");
          }
            contact.UserName = username;    
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
        public async Task<IActionResult> DeleteContact(string id, string username)
        {
            if (_context.Contact == null)
            {
                return NotFound();
            }
            var contact = await _context.Contact.Where(x => x.id == id && x.UserName == username).FirstOrDefaultAsync();
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/contacts/alice/messages
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<Object>> GetContactMessages(string id, string username)
        {
            if (_context.Message == null)
            {
                return new List<Object>();
            }

            var allMessages = await _context.Message.ToArrayAsync();

            var relaventMessages = from message in allMessages
                                   where message.contactName == id && message.UserName == username   
                                   select new {message.Id, message.Contect, message.Created, message.Sent};

            if (relaventMessages == null)
                return new List<Message>();
            return relaventMessages.ToArray();
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> PostMessage(string id, string username,
          [FromBody] Message message)
        {
            
            message.UserName = username;
            message.contactName = id;

            var contact = await _context.Contact.Where(_x => _x.id == id && _x.UserName == username).FirstOrDefaultAsync();
            contact.lastdate = message.Created;
            contact.last = message.Contect;
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/messages/{id2}")]
        public async Task<ActionResult<Object>> GetSpecificMessage(string id, int id2, string username)
        {

            var messages = await _context.Message.ToArrayAsync();
            var message = from m in messages
                          where m.UserName == username && m.contactName == id && m.Id == id2
                          select new { m.Id, m.Contect, m.Created, m.Sent }; ;

            if (message == null)
            {
                return NotFound();
            }

            return message.FirstOrDefault();
        }

        [HttpDelete("{id}/messages/{id2}")]
        public async Task<IActionResult> DeleteSpecificMessage(string id, int id2, string username)
        {
            var messages = await _context.Message.ToArrayAsync();
            var message = from m in messages
                          where m.UserName == username && m.contactName == id && m.Id == id2
                          select m;
            var mes = message.FirstOrDefault();
            if(mes != null)
            {
                _context.Message.Remove(mes);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        [HttpPut("{id}/messages/{id2}")]
        public async Task<IActionResult> PutSpecificMessage(string id, int id2, Message message, string username)
        {
            message.contactName = id;
            message.UserName = username;
            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ContactExists(string id)
        {
            return (_context.Contact?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
