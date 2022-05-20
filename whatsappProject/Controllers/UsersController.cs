using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        void UpdateUser(string Username, string nickname, string password, string image,string server,List<Contact>contacts);

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
        public List<Contact>  GetContacts(string Username)
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
                f.Password= password;
                f.Image= image;
                f.Server= server;
                f.Contacts = contacts;
            }
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _context;

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
            var user =  _context.GetUser(id);

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
            if(_context == null)
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
            user.Contacts=new List<Contact>();
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
}
