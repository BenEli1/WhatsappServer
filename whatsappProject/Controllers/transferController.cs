using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;
using whatsappProject.Hubs;
using whatsappProject.Data;
using net_core_api_push_notification_demo.Models;
using net_core_api_push_notification_demo.Services;



namespace whatsappProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class transferController : ControllerBase
    {
        private readonly whatsappProjectContext _context;
        private readonly ChatHub _hub;
        private readonly INotificationService _notificationService;

        public transferController(whatsappProjectContext context, ChatHub chathub, INotificationService notificationService)
        {
            _context = context;
            _hub = chathub;
            _notificationService = notificationService;
        }

        // GET: api/transfer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<transfer>>> Gettransfer()
        {
          if (_context.Transfer == null)
          {
              return NotFound();
          }
            return await _context.Transfer.ToArrayAsync();
        }

        // POST: api/transfer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<transfer>> Posttransfer([FromBody]transfer transfer)
        {

            //add the trasfer to the dataBase
            _context.Transfer.Add(transfer);
            await _context.SaveChangesAsync();

            //create message and push the message 
            Message message = new Message();
            message.contactName = transfer.from;
            message.UserName = transfer.to;
            message.Contect = transfer.content;
            message.Sent = "false";
            DateTime localDate = DateTime.Now;
            message.Created = localDate.ToString();
            _context.Message.Add(message);
            await _context.SaveChangesAsync();  

            //send sisnalr to react
            _hub.SendMessage(transfer.to, transfer.from, transfer.content);

            //find the Token of the username
            var UserToken = await _context.UserToken.FindAsync(transfer.to);
            string token = UserToken.Token;

            //create notification and send it
            NotificationModel notificationModel = new NotificationModel();

            notificationModel.Title = transfer.from + "|" + transfer.to;
            notificationModel.Body = transfer.content;
            notificationModel.DeviceId = token;
            notificationModel.IsAndroiodDevice = true;

            await _notificationService.SendNotification(notificationModel);

            return NoContent();
        }
    }
}
