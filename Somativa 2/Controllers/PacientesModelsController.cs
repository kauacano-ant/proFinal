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
    public class PacientesModelsController : Controller
    {
        private readonly SprintContext _context;

        public PacientesModelsController(SprintContext context)
        {
            _context = context;
        }

        // GET: PacientesModels
        public async Task<IActionResult> Index()
        {
            var sprintContext = _context.Paciente.Include(p => p.PlanodeSaude);
            return View(await sprintContext.ToListAsync());
        }

        // GET: PacientesModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var pacientesModel = await _context.Paciente
                .Include(p => p.PlanodeSaude)
                .FirstOrDefaultAsync(m => m.PacienteId == id);
            if (pacientesModel == null)
            {
                return NotFound();
            }

            return View(pacientesModel);
        }

        // GET: PacientesModels/Create
        public IActionResult Create()
        {
            ViewData["PlanodeSaudeId"] = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "PlanodeSaudeId");
            return View();
        }

        // POST: PacientesModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PacienteId,Nome,CPF,RG,Data_de_nascimento,Endereco,Telefone,PlanodeSaudeId")] PacientesModel pacientesModel)
        {
            if (ModelState.IsValid)
            {
                pacientesModel.PacienteId = Guid.NewGuid();
                _context.Add(pacientesModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanodeSaudeId"] = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "PlanodeSaudeId", pacientesModel.PlanodeSaudeId);
            return View(pacientesModel);
        }

        // GET: PacientesModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var pacientesModel = await _context.Paciente.FindAsync(id);
            if (pacientesModel == null)
            {
                return NotFound();
            }
            ViewData["PlanodeSaudeId"] = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "PlanodeSaudeId", pacientesModel.PlanodeSaudeId);
            return View(pacientesModel);
        }

        // POST: PacientesModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PacienteId,Nome,CPF,RG,Data_de_nascimento,Endereco,Telefone,PlanodeSaudeId")] PacientesModel pacientesModel)
        {
            if (id != pacientesModel.PacienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacientesModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacientesModelExists(pacientesModel.PacienteId))
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
            ViewData["PlanodeSaudeId"] = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "PlanodeSaudeId", pacientesModel.PlanodeSaudeId);
            return View(pacientesModel);
        }

        // GET: PacientesModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var pacientesModel = await _context.Paciente
                .Include(p => p.PlanodeSaude)
                .FirstOrDefaultAsync(m => m.PacienteId == id);
            if (pacientesModel == null)
            {
                return NotFound();
            }

            return View(pacientesModel);
        }

        // POST: PacientesModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Paciente == null)
            {
                return Problem("Entity set 'SprintContext.Paciente'  is null.");
            }
            var pacientesModel = await _context.Paciente.FindAsync(id);
            if (pacientesModel != null)
            {
                _context.Paciente.Remove(pacientesModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacientesModelExists(Guid id)
        {
          return (_context.Paciente?.Any(e => e.PacienteId == id)).GetValueOrDefault();
        }
    }
}
