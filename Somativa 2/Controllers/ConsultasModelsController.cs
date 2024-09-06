using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Somativa_2.Data;
using Somativa_2.Models;

namespace Somativa_2.Controllers
{
    public class ConsultasModelsController : Controller
    {
        private readonly SprintContext _context;

        public ConsultasModelsController(SprintContext context)
        {
            _context = context;
        }

        // GET: ConsultasModels
        public async Task<IActionResult> Index()
        {
            var sprintContext = _context.Consultas.Include(c => c.Consultorio);
            return View(await sprintContext.ToListAsync());
        }

        // GET: ConsultasModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Consultas == null)
            {
                return NotFound();
            }

            var consultasModel = await _context.Consultas
                .Include(c => c.Consultorio)
                .FirstOrDefaultAsync(m => m.ConsultaId == id);
            if (consultasModel == null)
            {
                return NotFound();
            }

            return View(consultasModel);
        }

        // GET: ConsultasModels/Create
        public IActionResult Create()
        {
            ViewData["ConsultorioId"] = new SelectList(_context.Consultorios, "ConsultorioId", "ConsultorioId");
            return View();
        }

        // POST: ConsultasModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsultaId,DataConsultas,Hora,ConsultorioId,PacienteId")] ConsultasModel consultasModel)
        {
            if (ModelState.IsValid)
            {
                consultasModel.ConsultaId = Guid.NewGuid();
                _context.Add(consultasModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsultorioId"] = new SelectList(_context.Consultorios, "ConsultorioId", "ConsultorioId", consultasModel.ConsultorioId);
            return View(consultasModel);
        }

        // GET: ConsultasModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Consultas == null)
            {
                return NotFound();
            }

            var consultasModel = await _context.Consultas.FindAsync(id);
            if (consultasModel == null)
            {
                return NotFound();
            }
            ViewData["ConsultorioId"] = new SelectList(_context.Consultorios, "ConsultorioId", "ConsultorioId", consultasModel.ConsultorioId);
            return View(consultasModel);
        }

        // POST: ConsultasModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ConsultaId,DataConsultas,Hora,ConsultorioId,PacienteId")] ConsultasModel consultasModel)
        {
            if (id != consultasModel.ConsultaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultasModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultasModelExists(consultasModel.ConsultaId))
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
            ViewData["ConsultorioId"] = new SelectList(_context.Consultorios, "ConsultorioId", "ConsultorioId", consultasModel.ConsultorioId);
            return View(consultasModel);
        }

        // GET: ConsultasModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Consultas == null)
            {
                return NotFound();
            }

            var consultasModel = await _context.Consultas
                .Include(c => c.Consultorio)
                .FirstOrDefaultAsync(m => m.ConsultaId == id);
            if (consultasModel == null)
            {
                return NotFound();
            }

            return View(consultasModel);
        }

        // POST: ConsultasModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Consultas == null)
            {
                return Problem("Entity set 'SprintContext.Consultas'  is null.");
            }
            var consultasModel = await _context.Consultas.FindAsync(id);
            if (consultasModel != null)
            {
                _context.Consultas.Remove(consultasModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultasModelExists(Guid id)
        {
          return (_context.Consultas?.Any(e => e.ConsultaId == id)).GetValueOrDefault();
        }
    }
}
