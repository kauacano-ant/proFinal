using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Somativa_2.Data;
using Somativa_2.Models;

namespace Somativa_2.Controllers
{
	public class PlanodeSaudeModelsController : Controller
	{
		private readonly SprintContext _context;
		private string _caminho;

		public PlanodeSaudeModelsController(SprintContext context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_caminho = hostEnvironment.WebRootPath;
		}

		// GET: PlanodeSaudeModels
		public async Task<IActionResult> Index()
		{
			return _context.PlanodeSaude != null ?
						View(await _context.PlanodeSaude.ToListAsync()) :
						Problem("Entity set 'SprintContext.PlanodeSaude'  is null.");
		}

		// GET: PlanodeSaudeModels/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null || _context.PlanodeSaude == null)
			{
				return NotFound();
			}

			var planodeSaudeModel = await _context.PlanodeSaude
				.FirstOrDefaultAsync(m => m.PlanodeSaudeId == id);
			if (planodeSaudeModel == null)
			{
				return NotFound();
			}

			return View(planodeSaudeModel);
		}

		// GET: PlanodeSaudeModels/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: PlanodeSaudeModels/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("PlanodeSaudeId,Nome,Telefone,Email,img,CNPJ")] PlanodeSaudeModel planodeSaudeModel, IFormFile imgUp)
		{
			planodeSaudeModel.PlanodeSaudeId = Guid.NewGuid();
			if (imgUp != null && imgUp.Length > 0)
			{
				string uploadsFolder = Path.Combine(_caminho, "img");

				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				string uniqueFileName = Guid.NewGuid().ToString() + "_" + imgUp.FileName;

				string filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await imgUp.CopyToAsync(fileStream);
				}
				planodeSaudeModel.img = uniqueFileName;
			}
			if (ModelState.IsValid)
			{


				_context.Add(planodeSaudeModel);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(planodeSaudeModel);
		}

		// GET: PlanodeSaudeModels/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null || _context.PlanodeSaude == null)
			{
				return NotFound();
			}

			var planodeSaudeModel = await _context.PlanodeSaude.FindAsync(id);
			if (planodeSaudeModel == null)
			{
				return NotFound();
			}
			return View(planodeSaudeModel);
		}

		// POST: PlanodeSaudeModels/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [Bind("PlanodeSaudeId,Nome,Telefone,CNPJ,Email")] PlanodeSaudeModel planodeSaudeModel, IFormFile imgUp)
		{
			if (id != planodeSaudeModel.PlanodeSaudeId)
			{
				return NotFound();
			}
			// Verifica se já existe uma imagem associada ao usuário
			var planodeSaudeModel2 = await _context.PlanodeSaude.FindAsync(id);
			if (planodeSaudeModel2 == null)
			{
				return NotFound();
			}

			// Se uma nova imagem foi carregada, excluir a antiga e salvar a nova
			if (imgUp != null && imgUp.Length > 0)
			{
				// Se houver uma ImagemPerfil existente, excluí-la
				if (!string.IsNullOrEmpty(planodeSaudeModel2.img))
				{
					string existingFilePath = Path.Combine(_caminho, "img", planodeSaudeModel2.img);
					if (System.IO.File.Exists(existingFilePath))
					{
						System.IO.File.Delete(existingFilePath);
					}
				}

				// Salvar a nova imagem
				string uploadsFolder = Path.Combine(_caminho, "img");
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				string uniqueFileName = Guid.NewGuid().ToString() + "_" + imgUp.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await imgUp.CopyToAsync(fileStream);
				}

				planodeSaudeModel2.img = uniqueFileName;
			}


			// Atualiza as informações do usuário
			if (ModelState.IsValid)
			{
				try
				{
					planodeSaudeModel2.Nome = planodeSaudeModel.Nome;
					planodeSaudeModel2.Telefone = planodeSaudeModel.Telefone;
					planodeSaudeModel2.CNPJ = planodeSaudeModel.CNPJ;
					planodeSaudeModel2.Email = planodeSaudeModel.Email;

					_context.Update(planodeSaudeModel2);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PlanodeSaudeModelExists(planodeSaudeModel2.PlanodeSaudeId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				_context.Update(planodeSaudeModel2);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(planodeSaudeModel2);

			//if (ModelState.IsValid)
			//{
			//    try
			//    {
			//        _context.Update(planodeSaudeModel);
			//        await _context.SaveChangesAsync();
			//    }
			//    catch (DbUpdateConcurrencyException)
			//    {
			//        if (!PlanodeSaudeModelExists(planodeSaudeModel.PlanodeSaudeId))
			//        {
			//            return NotFound();
			//        }
			//        else
			//        {
			//            throw;
			//        }
			//    }
			//    return RedirectToAction(nameof(Index));
			//}
		}

		// GET: PlanodeSaudeModels/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null || _context.PlanodeSaude == null)
			{
				return NotFound();
			}

			var planodeSaudeModel = await _context.PlanodeSaude
				.FirstOrDefaultAsync(m => m.PlanodeSaudeId == id);
			if (planodeSaudeModel == null)
			{
				return NotFound();
			}

			return View(planodeSaudeModel);
		}

		// POST: PlanodeSaudeModels/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			if (_context.PlanodeSaude == null)
			{
				return Problem("Entity set 'SprintContext.PlanodeSaude'  is null.");
			}
			var planodeSaudeModel = await _context.PlanodeSaude.FindAsync(id);

			if (planodeSaudeModel.img != null && planodeSaudeModel.img.Length > 0)
			{
				string uploadsFolder = Path.Combine(_caminho, "img");

				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				string filePath = Path.Combine(uploadsFolder, planodeSaudeModel.img);


				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
			}
			if (planodeSaudeModel != null)
			{
				_context.PlanodeSaude.Remove(planodeSaudeModel);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool PlanodeSaudeModelExists(Guid id)
		{
			return (_context.PlanodeSaude?.Any(e => e.PlanodeSaudeId == id)).GetValueOrDefault();
		}

	}
}
