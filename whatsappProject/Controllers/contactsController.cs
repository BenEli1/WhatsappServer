using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;

namespace whatsappProject.Controllers

{ 
[Route("api/[controller]")]
    [ApiController]
    public class contactsController : ControllerBase
    {
        private readonly IUserService _context;
        public contactsController(IUserService service)
        {
            _context = service;
        }

        public class UserSession
        {
            public string username { get; set; }    
        }

        [HttpGet("conected")]
        public string GetUserConnected()
        {
            var username = HttpContext.Session.GetString("username");
            return username;

        }

        // GET: api/contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetContactSecondSide(string username)
        {

          //string username = HttpContext.Session.GetString("username");

          if (_context.GetContacts(username) == null)
          {
              return NotFound();
          }

            var list = _context.GetContacts(username)
                        .Where(x=>true)
                        .Select (x => new { x.id, x.name, x.server, x.last, x.lastdate }).ToArray();

            return list;
        }

        // GET: api/contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetContact(string id)
        {

            string username = HttpContext.Session.GetString("username");
            if (_context.GetContacts(username) == null)
          {
              return NotFound();
          }

            var contact = _context.GetContacts(username)
                        .Where(x=>x.id == id)
                        .Select(x => new { x.id, x.name, x.server, x.last, x.lastdate }).ToArray();

            if (contact == null || contact.Length == 0)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(string id, Contact contact, string username)
        {

            if (id != contact.id)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
           string username = HttpContext.Session.GetString("username");

            if (_context.GetContacts(username) == null)
          {
              return Problem("Entity set 'whatsappProjectContext.Contact'  is null.");
          }
         _context.GetContacts(username).Add(contact);


            return CreatedAtAction("GetContact", new { id = contact.id }, contact);
        }

        // DELETE: api/contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            string username = HttpContext.Session.GetString("username");
            if (_context.GetContacts(username) == null)
            {
                return NotFound();
            }

            var contact =  _context.GetContacts(username).Find(x=>x.id==id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.GetContacts(username).Remove(contact);

            return NoContent();
        }

        private bool ContactExists(string id)
        {
            string username = HttpContext.Session.GetString("username");
            return (_context.GetContacts(username).Any(e => e.id == id));
        }

        // GET: api/contacts/alice/messages
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<Object>> GetContactMessages(string id, string username)
        {

            //string username = HttpContext.Session.GetString("username");

            if (_context.GetContacts(username) == null)
            {
                return NotFound();
            }

            var contact = _context.GetContacts(username)
                        .Where(x=>x.id == id)
                        .Select(x => x.Messages).ToArray();

            if (contact == null || contact.Length == 0)
            {
                return NotFound();
            }

            if(contact == null)
                return new List<Message>();


            return contact;
        }

 
      

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> PostMessage(string id, string username, 
            [FromBody]Message message)
        {
            var contact = _context.GetContacts(username).Find(x => x.id == id);

            if (contact == null)
            {
                return BadRequest();
            }

            contact.last = message.Text;
            contact.lastdate = message.Date;

            contact.Messages.Add(message);

            //_context.Message.Add(message);
                
            return await PutContact(id, contact, username);
        }

        [HttpGet("{id}/messages/{id2}")]
        public async Task<ActionResult<Object>> GetSpecificMessage(string id, int id2)
        {

            //string username = HttpContext.Session.GetString("username");
            string username = "sahar";
            if (_context.GetContacts(username) == null)
            {
                return NotFound();
            }

            var contact = _context.GetContacts(username)
                        .Where(x=>x.id == id)
                        .Select(x => new { x.Messages }).ToArray();

            if (contact == null || contact.Length == 0)
            {
                return NotFound();
            }
            return contact;
        }

        [HttpDelete("{id}/messages/{id2}")]
        public async Task<IActionResult> DeleteSpecificMessage(string id, int id2)
        {
            string username = HttpContext.Session.GetString("username");

            var contact = _context.GetContacts(username).Find(x => x.id == id);

            if (contact == null)
            {
                return BadRequest();
            }

            Message toDelete = contact.Messages.Find(x => x.Id == id2);

            if (toDelete != null)
            {
                contact.Messages.Remove(toDelete);
            }

            return await PutContact(id, contact, username);
        }

        [HttpPut("{id}/messages/{id2}")]
        public async Task<IActionResult> PutSpecificMessage(string id, int id2, Message message)
        {
            string username = HttpContext.Session.GetString("username");

            var contact = _context.GetContacts(username).Find(x => x.id == id);

            if (contact == null)
            {
                return BadRequest();
            }

            Message toDelete = contact.Messages.Find(x => x.Id == id2);

            if (toDelete != null)
            {
                contact.Messages.Remove(toDelete);
                contact.Messages.Add(message);
            }
            return await PutContact(id, contact, username);
        }
    }
}
