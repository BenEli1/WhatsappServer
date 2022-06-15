using Microsoft.AspNetCore.Mvc;
using whatsappProject.Models;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Hubs;
using whatsappProject.Data;
using net_core_api_push_notification_demo.Models;
using net_core_api_push_notification_demo.Services;


namespace whatsappProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class invitationsController : ControllerBase
    {
        private readonly whatsappProjectContext _context;
        private readonly ChatHub _hub;
        private readonly INotificationService _notificationService;

        public invitationsController(whatsappProjectContext context, ChatHub chathub, INotificationService notificationService)
        {
            _context = context;
            _hub = chathub;
            _notificationService = notificationService;
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
        public async Task<ActionResult<Invitation>> PostInvitation([FromBody]Invitation invitation)
        {
            //send signalr to react
            //_hub.SendContact(invitation.to, invitation.from, invitation.server);

            //add the invitation to database
            _context.Invitation.Add(invitation);
            await _context.SaveChangesAsync();

            //create new Contact and insert to database
            Contact contact = new Contact();
            contact.server = invitation.server;
            contact.UserName = invitation.to;
            contact.id = invitation.from;
            contact.name = invitation.from;
            contact.last = "";
            contact.lastdate = "";
            _context.Contact.Add(contact);
            await _context.SaveChangesAsync();

            //find the Token of the username
            var UserToken = await _context.UserToken.FindAsync(invitation.from);
            string token = UserToken.Token;

            //create notification and send it
            NotificationModel notificationModel = new NotificationModel();

            notificationModel.Title = "Invitaion";
            notificationModel.Body = invitation.from + "|" + invitation.to;
            notificationModel.DeviceId = token;
            notificationModel.IsAndroiodDevice = true;

            await _notificationService.SendNotification(notificationModel);

            return NoContent();
        }
    }
}
