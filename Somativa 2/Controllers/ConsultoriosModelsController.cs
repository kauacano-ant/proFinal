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
    public class ConsultoriosModelsController : Controller
    {
        private readonly SprintContext _context;

        public ConsultoriosModelsController(SprintContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Consultorios()
        {
            var Consultorios = await _context.Consultorios.Include(c => c.Especialidade).ToListAsync();
            ViewData["Categ"] = await _context.Consultorios.ToListAsync();

            return View(Consultorios);
        }

        public async Task<IActionResult> SearchProd(Guid inCategoria, string inNome)
        {
            var Consultorios = await _context.Consultorios.ToListAsync();

            if (!string.IsNullOrEmpty(inNome))
            {
                inNome = inNome.Trim().ToUpper();
                Consultorios = Consultorios.Where(i => i.Nome.ToUpper().Contains(inNome)).ToList();
            }

            if (inCategoria != Guid.Empty)
            {
                Consultorios = Consultorios.Where(i => i.ConsultorioId == inCategoria).ToList();
            }

            ViewData["Categ"] = await _context.Consultorios.ToListAsync();
            return View("Index", Consultorios);
        }


        // GET: ConsultoriosModels
        public async Task<IActionResult> Index()
        {
              return _context.Consultorios != null ? 
                          View(await _context.Consultorios.ToListAsync()) :
                          Problem("Entity set 'SprintContext.Consultorios'  is null.");
        }

        // GET: ConsultoriosModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                return NotFound();
            }

            var consultoriosModel = await _context.Consultorios
                .FirstOrDefaultAsync(m => m.ConsultorioId == id);
            if (consultoriosModel == null)
            {
                return NotFound();
            }

            return View(consultoriosModel);
        }

        // GET: ConsultoriosModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConsultoriosModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsultorioId,Nome,Endereco,Telefone,Especialidade,Email")] ConsultoriosModel consultoriosModel)
        {
            if (ModelState.IsValid)
            {
                consultoriosModel.ConsultorioId = Guid.NewGuid();
                _context.Add(consultoriosModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consultoriosModel);
        }

        // GET: ConsultoriosModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                return NotFound();
            }

            var consultoriosModel = await _context.Consultorios.FindAsync(id);
            if (consultoriosModel == null)
            {
                return NotFound();
            }
            return View(consultoriosModel);
        }

        // POST: ConsultoriosModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ConsultorioId,Nome,Endereco,Telefone,Especialidade,Email")] ConsultoriosModel consultoriosModel)
        {
            if (id != consultoriosModel.ConsultorioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultoriosModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultoriosModelExists(consultoriosModel.ConsultorioId))
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
            return View(consultoriosModel);
        }

        // GET: ConsultoriosModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                return NotFound();
            }

            var consultoriosModel = await _context.Consultorios
                .FirstOrDefaultAsync(m => m.ConsultorioId == id);
            if (consultoriosModel == null)
            {
                return NotFound();
            }

            return View(consultoriosModel);
        }

        // POST: ConsultoriosModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Consultorios == null)
            {
                return Problem("Entity set 'SprintContext.Consultorios'  is null.");
            }
            var consultoriosModel = await _context.Consultorios.FindAsync(id);
            if (consultoriosModel != null)
            {
                _context.Consultorios.Remove(consultoriosModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultoriosModelExists(Guid id)
        {
          return (_context.Consultorios?.Any(e => e.ConsultorioId == id)).GetValueOrDefault();
        }
    }
}
