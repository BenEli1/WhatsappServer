using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;

namespace whatsappProject.Controllers
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string Username);

        void DeleteUser(string Username);

        void UpdateUser(string Username, string nickname, string password, string image, string server, List<Contact> contacts);

        void CreateUser(User User);
        List<Contact> GetContacts(string Username);
    }

    public class UserService : IUserService
    {
        /*  public string id { get; set; }
          public string name { get; set; }
          public string server { get; set; }
          public string last { get; set; }
          public string lastdate { get; set; }
          public List<Message> Messages { get; set; }*/
        private static List<User> _Users = new List<User>();
        public void CreateUser(User User)
        {
            _Users.Add(User);
        }

        public void DeleteUser(string Username)
        {
            User f = _Users.Find(x => x.UserName == Username);
            if (f != null)
            {
                _Users.Remove(_Users.Find(x => x.UserName == Username));
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _Users;
        }

        public User GetUser(string Username)
        {
            return _Users.Find(x => x.UserName == Username);

        }
        public List<Contact> GetContacts(string Username)
        {
            return _Users.Find(x => x.UserName == Username).Contacts;

        }

        public void UpdateUser(string Username, string nickname, string password, string image, string server, List<Contact> contacts)
        {
            User f = GetUser(Username);
            if (f != null)
            {
                f.UserName = Username;
                f.NickName = nickname;
                f.Password = password;
                f.Image = image;
                f.Server = server;
                f.Contacts = contacts;
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class contactsController : ControllerBase
    {
        private readonly IUserService _context;

        // GET: api/contacts/Users
        [HttpGet("Users")]
        public IEnumerable<User> getUserList()
        {
            if (_context == null)
            {
                return (IEnumerable<User>)NotFound();
            }
            var users = _context.GetAllUsers();
            return users;
        }

        // GET: api/contacts/Users/5
        [HttpGet("Users/{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            if (_context == null)
            {
                return NotFound();
            }
            var user = _context.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/contacts/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Users/{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.UserName)
            {
                return BadRequest();
            }


            try
            {
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/contacts/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Users")]
        public async Task<ActionResult<User>> PostUser([Bind("UserName", "NickName", "Password", "Image", "Server")] User user)
        {
            if (_context == null)
            {
                return NotFound();
            }

            /*User user = new();
            user.UserName = UserName;
            user.NickName = NickName;
            user.Password = Password;
            user.Image = Image;
            user.Server = Server;
            user.Users = new List<User>();*/
            user.Contacts = new List<Contact>();
            _context.CreateUser(user);

            return CreatedAtAction("GetUser", new { id = user.UserName }, user);
        }

        // DELETE: api/contacts/Users/5
        [HttpDelete("Users/{id}")]
        public async Task<IActionResult> DeleteCertainUser(string id)
        {
            if (_context == null)
            {
                return NotFound();
            }
            var user = _context.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.DeleteUser(id);

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return (_context.GetAllUsers().Any(e => e.UserName == id));
        }


        public class UserSession
        {
            public string username { get; set; }
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
                        .Where(x => true)
                        .Select(x => new { x.id, x.name, x.server, x.last, x.lastdate }).ToArray();

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
                        .Where(x => x.id == id)
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

            var contact = _context.GetContacts(username).Find(x => x.id == id);
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
                        .Where(x => x.id == id)
                        .Select(x => x.Messages).ToArray();

            if (contact == null || contact.Length == 0)
            {
                return NotFound();
            }

            if (contact == null)
                return new List<Message>();


            return contact;
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> PostMessage(string id, string username,
            [FromBody] Message message)
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
                        .Where(x => x.id == id)
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
