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
    public class FeedbackModelsController : Controller
    {
        private readonly SprintContext _context;

        public FeedbackModelsController(SprintContext context)
        {
            _context = context;
        }

        // GET: FeedbackModels
        public async Task<IActionResult> Index()
        {
            var sprintContext = _context.Feedback.Include(f => f.Consulta);
            return View(await sprintContext.ToListAsync());
        }

        // GET: FeedbackModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Feedback == null)
            {
                return NotFound();
            }

            var feedbackModel = await _context.Feedback
                .Include(f => f.Consulta)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (feedbackModel == null)
            {
                return NotFound();
            }

            return View(feedbackModel);
        }

        // GET: FeedbackModels/Create
        public IActionResult Create()
        {
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "ConsultaId", "ConsultaId");
            return View();
        }

        // POST: FeedbackModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackId,Comentario,Nota,ConsultaId")] FeedbackModel feedbackModel)
        {
            if (ModelState.IsValid)
            {
                feedbackModel.FeedbackId = Guid.NewGuid();
                _context.Add(feedbackModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "ConsultaId", "ConsultaId", feedbackModel.ConsultaId);
            return View(feedbackModel);
        }

        // GET: FeedbackModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Feedback == null)
            {
                return NotFound();
            }

            var feedbackModel = await _context.Feedback.FindAsync(id);
            if (feedbackModel == null)
            {
                return NotFound();
            }
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "ConsultaId", "ConsultaId", feedbackModel.ConsultaId);
            return View(feedbackModel);
        }

        // POST: FeedbackModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FeedbackId,Comentario,Nota,ConsultaId")] FeedbackModel feedbackModel)
        {
            if (id != feedbackModel.FeedbackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedbackModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackModelExists(feedbackModel.FeedbackId))
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
            ViewData["ConsultaId"] = new SelectList(_context.Consultas, "ConsultaId", "ConsultaId", feedbackModel.ConsultaId);
            return View(feedbackModel);
        }

        // GET: FeedbackModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Feedback == null)
            {
                return NotFound();
            }

            var feedbackModel = await _context.Feedback
                .Include(f => f.Consulta)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (feedbackModel == null)
            {
                return NotFound();
            }

            return View(feedbackModel);
        }

        // POST: FeedbackModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Feedback == null)
            {
                return Problem("Entity set 'SprintContext.Feedback'  is null.");
            }
            var feedbackModel = await _context.Feedback.FindAsync(id);
            if (feedbackModel != null)
            {
                _context.Feedback.Remove(feedbackModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackModelExists(Guid id)
        {
          return (_context.Feedback?.Any(e => e.FeedbackId == id)).GetValueOrDefault();
        }
    }
}
