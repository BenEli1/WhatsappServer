using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Models;
using whatsappProject.Controllers;
using whatsappProject.Hubs;

namespace whatsappProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class transferController : ControllerBase
    {
        private readonly IUserService _context;
        private ChatHub _hub;

        public transferController(IUserService context, ChatHub chathub)
        {
            _context = context;
            _hub = chathub;
        }

        // GET: api/transfer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<transfer>>> Gettransfer()
        {
          if (_context.GetAllTransfers() == null)
          {
              return NotFound();
          }
            return _context.GetAllTransfers();
        }

        // GET: api/transfer/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<transfer>> Gettransfer(int id)
        {
          if (_context.transfer == null)
          {
              return NotFound();
          }
            var transfer = await _context.transfer.FindAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }

            return transfer;
        }

        // PUT: api/transfer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Puttransfer(int id, transfer transfer)
        {
            if (id != transfer.id)
            {
                return BadRequest();
            }

            _context.Entry(transfer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!transferExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/transfer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<transfer>> Posttransfer([FromBody]transfer transfer)
        {
            _context.AddTransfer(transfer);

            _hub.SendMessage(transfer.to, transfer.from, transfer.content);

            return NoContent();
        }

        // DELETE: api/transfer/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Deletetransfer(int id)
        {
            if (_context.transfer == null)
            {
                return NotFound();
            }
            var transfer = await _context.transfer.FindAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }

            _context.transfer.Remove(transfer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool transferExists(int id)
        {
            return (_context.transfer?.Any(e => e.id == id)).GetValueOrDefault();
        }*/
    }
}
