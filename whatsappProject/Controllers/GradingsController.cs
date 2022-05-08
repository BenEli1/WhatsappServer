#nullable disable
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
    public class GradingsController : Controller
    {
        private readonly whatsappProjectContext _context;

        public GradingsController(whatsappProjectContext context)
        {
            _context = context;
        }

        // GET: Gradings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Grading.ToListAsync());
        }

        // GET: Gradings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grading = await _context.Grading
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grading == null)
            {
                return NotFound();
            }

            return View(grading);
        }

        // GET: Gradings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gradings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score,Name,FeedBack")] Grading grading)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grading);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grading);
        }

        // GET: Gradings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grading = await _context.Grading.FindAsync(id);
            if (grading == null)
            {
                return NotFound();
            }
            return View(grading);
        }

        // POST: Gradings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Score,Name,FeedBack")] Grading grading)
        {
            if (id != grading.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grading);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradingExists(grading.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(grading);
        }

        // GET: Gradings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grading = await _context.Grading
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grading == null)
            {
                return NotFound();
            }

            return View(grading);
        }

        // POST: Gradings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grading = await _context.Grading.FindAsync(id);
            _context.Grading.Remove(grading);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradingExists(int id)
        {
            return _context.Grading.Any(e => e.Id == id);
        }
    }
}
