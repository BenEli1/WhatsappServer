using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Data;
using whatsappProject.Models;

namespace whatsappProject.Controllers
{
    public interface IUsersService
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string username);
        void DeleteUser(string username);
        void AddUser(User user);
    }
    public class UserService : IUsersService
    {
        private static List<User> _users = new List<User>();

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }
        public User GetUser(string username)
        {
            return _users.Find(x => x.UserName == username);
        }
        public void DeleteUser(string username)
        {
            _users.Remove(GetUser(username));
        }
        public void AddUser(User user)
        {
            _users.Add(user);
        }
    }

    public class UsersController : Controller
    {
        private readonly IUsersService _service;

        public UsersController(IUsersService service)
        {
            _service = service;
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string username)
        {
            User user = _service.GetUser(username);
            return Json(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task Create([Bind("UserName,NickName,Password,Image")] User user)
        {
            _service.AddUser(user);
        }

        [HttpPost]
        public async Task Login(string UserName,string Password)
        {
            HttpContext.Session.SetString("username", UserName);
        }
    }
}
