using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Somativa_2.Data;
using Somativa_2.Models;

namespace Somativa_2.Controllers
{
    public class CategsController : Controller
    {
        private readonly SprintContext _context;

        public CategsController(SprintContext context)
        {
            _context = context;
        }

        // GET: Categs
        public async Task<IActionResult> Index()
        {
              return _context.Categ != null ? 
                          View(await _context.Categ.ToListAsync()) :
                          Problem("Entity set 'SprintContext.Categ'  is null.");
        }

        // GET: Categs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Categ == null)
            {
                return NotFound();
            }

            var categ = await _context.Categ
                .FirstOrDefaultAsync(m => m.CategId == id);
            if (categ == null)
            {
                return NotFound();
            }

            return View(categ);
        }

        // GET: Categs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategId,Nome")] Categ categ)
        {
            if (ModelState.IsValid)
            {
                categ.CategId = Guid.NewGuid();
                _context.Add(categ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categ);
        }

        // GET: Categs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Categ == null)
            {
                return NotFound();
            }

            var categ = await _context.Categ.FindAsync(id);
            if (categ == null)
            {
                return NotFound();
            }
            return View(categ);
        }

        // POST: Categs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CategId,Nome")] Categ categ)
        {
            if (id != categ.CategId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategExists(categ.CategId))
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
            return View(categ);
        }

        // GET: Categs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Categ == null)
            {
                return NotFound();
            }

            var categ = await _context.Categ
                .FirstOrDefaultAsync(m => m.CategId == id);
            if (categ == null)
            {
                return NotFound();
            }

            return View(categ);
        }

        // POST: Categs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Categ == null)
            {
                return Problem("Entity set 'SprintContext.Categ'  is null.");
            }
            var categ = await _context.Categ.FindAsync(id);
            if (categ != null)
            {
                _context.Categ.Remove(categ);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategExists(Guid id)
        {
          return (_context.Categ?.Any(e => e.CategId == id)).GetValueOrDefault();
        }
    }
}
