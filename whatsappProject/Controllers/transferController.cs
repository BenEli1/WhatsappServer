using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Data;
using whatsappProject.Models;

namespace whatsappProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class transferController : ControllerBase
    {
        private readonly whatsappProjectContext _context;

        public transferController(whatsappProjectContext context)
        {
            _context = context;
        }

        // GET: api/transfer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<transfer>>> Gettransfer()
        {
          if (_context.transfer == null)
          {
              return NotFound();
          }
            return await _context.transfer.ToListAsync();
        }

        // GET: api/transfer/5
        [HttpGet("{id}")]
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
        }

        // POST: api/transfer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<transfer>> Posttransfer(transfer transfer)
        {
          if (_context.transfer == null)
          {
              return Problem("Entity set 'whatsappProjectContext.transfer'  is null.");
          }
            _context.transfer.Add(transfer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gettransfer", new { id = transfer.id }, transfer);
        }

        // DELETE: api/transfer/5
        [HttpDelete("{id}")]
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
        }
    }
}
