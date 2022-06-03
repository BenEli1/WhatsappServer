using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;
using whatsappProject.Data;


/*namespace whatsappProject.Controllers
{


    /// <summary>
    /// /////////////////////  the interface ////////////////////////////////////////////////////////////
    /// </summary>
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllUsers();

        public Task<User> GetUser(string Username);

        public void DeleteUser(string Username);

        public void UpdateUser(string Username, string nickname, string password, string image, string server, List<Contact> contacts);
        
        public void CreateUser(User User);
        
        public Task<List<Contact>> GetContacts(string Username);

        public Task<List<Message>> GetMessages(string Username, string contactName);

        public Task<Contact> GetContact(string Username, string contactName);

        public void AddContact(string username, Contact Contact);

        public void AddMessage(Message Message, string username, string contactName);

        public void AddTransfer(transfer transfer);

        public Task<List<transfer>> GetAllTransfers();

        public void DeleteMessage(Message Message);

        public void AddInvitation(Invitation invitation);

        public Task<List<Invitation>> GetAllInvitations();
        public int getId();
    }


    /// <summary>
    /// ///////////////// the class implements the interface ////////////////////////////////////
    /// </summary>

    //----------------------------try to implements the interface with data base---------------
 
        public class DataBase : IUserService
        {
        private readonly whatsappProjectContext _context;
            private int id = 0;

            public DataBase(whatsappProjectContext db)
            {
              _context = db;
            }
            public async Task<IEnumerable<User>> GetAllUsers()
            {
                return await _context.User.ToArrayAsync();
            }

            public async Task<User> GetUser(string Username)
            {
                User user = await _context.User.FindAsync(Username);
                return user;  
            }

            public void DeleteUser(string Username)
            {
                
            }

            public void UpdateUser(string Username, string nickname, string password, string image, string server, List<Contact> contacts)
            {

            }

            public async void CreateUser(User User)
            {
                await _context.User.AddAsync(User);
                _context.SaveChanges();
            }

            public async Task<List<Contact>> GetContacts(string Username)
            {
                return await _context.Contact.Where(x => x.UserName == Username).ToListAsync(); 
            }

            public async Task<List<Message>> GetMessages(string Username, string contactName)
            {
                return await _context.Message.Where(x => x.UserName == Username 
                    && x.contactName == contactName).ToListAsync(); 
            }

            public async Task<Contact> GetContact(string Username, string contactName)
            { 
                return await _context.Contact.Where(x => x.UserName == Username && x.id == contactName).FirstOrDefaultAsync(); 
            }

            public async void AddContact(string username, Contact Contact)
            {
                Contact.UserName = username;
                await _context.Contact.AddAsync(Contact);
                _context.SaveChanges();

            }

            public async void AddMessage(Message Message, string username, string contactName)
            {
                Message.UserName = username;    
                Message.contactName = contactName;
                await _context.Message.AddAsync(Message);
                _context.SaveChanges();
            
            }

            public async void AddTransfer(transfer transfer)
            {
                await _context.Transfer.AddAsync(transfer);
                _context.SaveChanges();
            }

            public async  Task<List<transfer>> GetAllTransfers()
            {
                return await _context.Transfer.ToListAsync();
            }

            public async void AddInvitation(Invitation invitation)
            {
                await _context.Invitation.AddAsync(invitation);
                _context.SaveChanges();
            }

            public async Task<List<Invitation>> GetAllInvitations()
            {
                return await _context.Invitation.ToListAsync(); 
            }

            public void DeleteMessage(Message Message)
            {
                _context.Message.Remove(Message);
                _context.SaveChanges();
            }
            public int getId()
            {
                return ++id;
            }

    }
    /*
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private DataBase _context;

        public UsersController(DataBase context)
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

            return users.Result;
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

            return user.Result;
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
            return (_context.GetAllUsers().Result.Any(e => e.UserName == id));
        }
    }


/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class coctsController : ControllerBase
    {
        private DataBase _context;

        public coctsController(DataBase context)
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

            var list = _context.GetContacts(username).Result
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

            var contact = _context.GetContacts(username).Result
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
            Contact newContact = _context.GetContact(username, id).Result;
            newContact.id = contact.id; 
            newContact.name = contact.name; 
            newContact.server = contact.server;     
            newContact.last = contact.last; 
            newContact.lastdate = contact.lastdate; 

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
        public async Task<IActionResult> DeleteContact(string id, string username)
        {
            Contact contact = _context.GetContact(username, id).Result;

            if ( contact == null)
            {
                return NotFound();
            }

            _context.GetContacts(username).Result.Remove(contact);

            return NoContent();
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

            var messages = _context.GetMessages(username, id).Result;
            var message = messages.Find(x => x.Id == id2); 
            if (message == null)
            {
                return NotFound();
            }
            return message;
        }

        [HttpDelete("{id}/messages/{id2}")]
        public async Task<IActionResult> DeleteSpecificMessage(string id, int id2, string username)
        {
            var contact = _context.GetContact(username, id);

            if (contact == null)
            {
                return BadRequest();
            }

            var messages = _context.GetMessages(username, id).Result;
            var toDelete = messages.Find(x => x.Id == id2);

            if (toDelete != null)
            {
               _context.DeleteMessage(toDelete);
            }

            return NoContent();
        }
        
        [HttpPut("{id}/messages/{id2}")]
        public async Task<IActionResult> PutSpecificMessage(string id, int id2, Message message, string username)
        {

            var contact = _context.GetContact(username, id);

            if (contact == null)
            {
                return BadRequest();
            }

            var messages = _context.GetMessages(username, id).Result;
            var toDelete = messages.Find(x => x.Id == id2);


            if (toDelete != null)
            {
                _context.DeleteMessage(toDelete);
                _context.AddMessage(message, username, id);
            }
            return NoContent();
        }
    }
}*/
