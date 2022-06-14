using Microsoft.AspNetCore.Mvc;
using whatsappProject.Models;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Hubs;
using whatsappProject.Data;


namespace whatsappProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class invitationsController : ControllerBase
    {
        private readonly whatsappProjectContext _context;
        private readonly ChatHub _hub;

        public invitationsController(whatsappProjectContext context, ChatHub chathub)
        {
            _context = context;
            _hub = chathub;
        }

        // GET: api/invitations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invitation>>> GetInvitation()
        {
            return await _context.Invitation.ToArrayAsync();
        }       

        // POST: api/invitations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invitation>> PostInvitation(Invitation invitation)
        {
            _hub.SendContact(invitation.to, invitation.from, invitation.server);

            _context.Invitation.Add(invitation);
            await _context.SaveChangesAsync();
            Contact contact = new Contact();
            contact.server = invitation.server;
            contact.UserName = invitation.to;
            contact.id = invitation.from;
            contact.name = invitation.from;
            contact.last = "";
            contact.lastdate = "";
            _context.Contact.Add(contact);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
