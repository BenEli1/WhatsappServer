using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;

namespace whatsappProject.Controllers
{


    /// <summary>
    /// /////////////////////  the interface ////////////////////////////////////////////////////////////
    /// </summary>
    public interface IUserService
    {
        public IEnumerable<User> GetAllUsers();

        public User GetUser(string Username);

        public void DeleteUser(string Username);

        public void UpdateUser(string Username, string nickname, string password, string image, string server, List<Contact> contacts);
        
        public void CreateUser(User User);
        
        public List<Contact> GetContacts(string Username);

        public List<Message> GetMessages(string Username, string contactName);

        public Contact GetContact(string Username, string contactName);

        public void AddContact(string username, Contact Contact);

        public void AddMessage(Message Message, string username, string contactName);

        public void AddTransfer(transfer transfer);

        public List<transfer> GetAllTransfers();

        public void AddInvitation(Invitation invitation);

        public List<Invitation> GetAllInvitations();
        int getId();
    }


    /// <summary>
    /// ///////////////// the class implements the interface ////////////////////////////////////
    /// </summary>

    public class UserService : IUserService
    {
        private static List<User> _Users = new List<User>();
        private static List<transfer> _Transfer = new List<transfer>();
        private static List<Invitation> _Invitations = new List<Invitation>();
        private static int countingId = 0;


        public List<transfer> GetAllTransfers()
        {
            return _Transfer;
        }
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

        public void AddTransfer(transfer transfer)
        {
            _Transfer.Add(transfer);
        }

        public List<Message> GetMessages(string Username, string contactName)
        {
            Contact contact = GetContact(Username, contactName);
            if (contact == null) return null;
            return contact.Messages;
        }

        public Contact GetContact(string Username, string contactName)
        {
            User user = GetUser(Username);
            if (user == null)
                return null;

            Contact contact = user.Contacts.Find(x => x.id == contactName);
            return contact;
        }

        public void AddContact(string username, Contact Contact)
        {
            User user = GetUser(username);
            if (user.Contacts.Find(x => x.id == Contact.id) != null)
                return;
            Contact.Messages = new List<Message>();
            user.Contacts.Add(Contact);
        }

        public void AddMessage(Message message, string username, string contactName)
        {
            Contact contact = GetContact(username, contactName);
            contact.Messages.Add(message);
            contact.last = message.Text;
            contact.lastdate = message.Date;
        }
        public void AddInvitation(Invitation invitation)
        {
            _Invitations.Add(invitation);
        }

        public List<Invitation> GetAllInvitations()
        {
            return _Invitations;    
        }

        public int getId()
        {
            countingId++;
           return countingId;
        }
    }


    /// <summary>
    /// /////////////////////////////////   user controller ///////////////////////////////////////////////
    /// </summary>


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserService _context;

        public UsersController(IUserService context)
        {
            _context = context;
        }


        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> getUserList()
        {
            if (_context == null)
            {
                return (IEnumerable<User>)NotFound();
            }
            var users = _context.GetAllUsers();

            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
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

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([Bind("UserName", "NickName", "Password", "Image", "Server")] User user)
        {
            if (_context == null)
            {
                return NotFound();
            }

            user.Contacts = new List<Contact>();
            _context.CreateUser(user);

            return CreatedAtAction("GetUser", new { id = user.UserName }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
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
    }


/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class contactsController : ControllerBase
    {
        private IUserService _context;

        public contactsController(IUserService context)
        {
           _context = context;   
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
        public async Task<ActionResult<Object>> GetContact(string id, string username)
        {

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

            return contact[0];
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
        public async Task<ActionResult<Contact>> PostContact([FromBody]Contact contact, string username)
        {
            if (_context.GetContacts(username) == null)
            {
                return Problem("Entity set 'whatsappProjectContext.Contact'  is null.");
            }

            _context.AddContact(username, contact);

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

            var messages = _context.GetMessages(username, id);

            if (messages == null)
                return new List<Message>();
            return messages;
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> PostMessage(string id, string username,
            [FromBody] Message message)
        {
            message.Id = _context.getId();
            _context.AddMessage(message, username, id);

            return NoContent();
        }

        [HttpGet("{id}/messages/{id2}")]
        public async Task<ActionResult<Object>> GetSpecificMessage(string id, int id2, string username)
        {

            if (_context.GetContacts(username) == null)
            {
                return NotFound();
            }

            var contact = _context.GetContact(username, id);
            var message=contact.Messages.Find(x => x.Id == id2);
            if (message == null)
            {
                return NotFound();
            }
            return message;
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
