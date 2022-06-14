using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;
using whatsappProject.Hubs;
using whatsappProject.Data;


namespace whatsappProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class transferController : ControllerBase
    {
        private readonly whatsappProjectContext _context;
        private readonly ChatHub _hub;

        public transferController(whatsappProjectContext context, ChatHub chathub)
        {
            _context = context;
            _hub = chathub;
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
            _context.Transfer.Add(transfer);
            await _context.SaveChangesAsync();
            Message message = new Message();
            message.contactName = transfer.from;
            message.UserName = transfer.to;
            message.Contect = transfer.content;
            message.Sent = "false";
            DateTime localDate = DateTime.Now;
            message.Created = localDate.ToString();
            _context.Message.Add(message);
            await _context.SaveChangesAsync();  
            _hub.SendMessage(transfer.to, transfer.from, transfer.content);
            return NoContent();
        }
    }
}
