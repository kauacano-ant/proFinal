using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;

		public PlanodeSaudeModelsController(SprintContext context, IWebHostEnvironment hostEnvironment, SignInManager<IdentityUser> signInManager
			, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_caminho = hostEnvironment.WebRootPath;
			_signInManager = signInManager;
			_userManager = userManager;
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

			// Verifica se o usuário está autenticado
			if (_signInManager.IsSignedIn(User))
			{
				var user = await _userManager.GetUserAsync(User); // Obtém o usuário atual

				// Verifica se o usuário é "Admin"
				if (await _userManager.IsInRoleAsync(user, "Admin"))
				{

					return View(planodeSaudeModel);
				}
				else
				{
					// Se não for admin, carrega apenas o paciente associado ao usuário
					var pacientesModel = await _context.Paciente
						.Include(p => p.PlanodeSaude)
						.FirstOrDefaultAsync(p => p.UserId == user.Id);

					if (pacientesModel != null)
					{
						// Redireciona para a página de detalhes do paciente associado ao usuário
						return RedirectToAction("Details", "PacientesModels", new { id = pacientesModel.PacienteId });
					}
					else
					{
						// Se o paciente não existir, redireciona para a página de criação
						return Redirect("/PacientesModels/Create");
					}
				}
			}
			else
			{
				// Se o usuário não estiver autenticado, redireciona para a página de login
				return Redirect("/Identity/Account/Login");
			}


		}

		// GET: PlanodeSaudeModels/Create
		public IActionResult Create()
		{
            //ViewBag.PlanodeSaude = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "Nome");
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
			// Verifica a existência do plano de saúde antes de atribuir `id`
			var existingPlan = await _context.PlanodeSaude.FindAsync(planodeSaudeModel.PlanodeSaudeId);
			if (existingPlan == null)
			{
				return Json(new { success = false, message = "Plano de Saúde não encontrado." });
			}

			if (imgUp != null && imgUp.Length > 0)
			{
				await ProcessImageUploadAsync(existingPlan, imgUp);
			}

			if (ModelState.IsValid)
			{
				try
				{
					existingPlan.Nome = planodeSaudeModel.Nome;
					existingPlan.Telefone = planodeSaudeModel.Telefone;
					existingPlan.CNPJ = planodeSaudeModel.CNPJ;
					existingPlan.Email = planodeSaudeModel.Email;

					_context.Update(existingPlan);
					await _context.SaveChangesAsync();

					return Json(new { success = true, message = "Plano de Saúde atualizado com sucesso." });
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PlanodeSaudeModelExists(existingPlan.PlanodeSaudeId))
					{
						return Json(new { success = false, message = "Plano de Saúde não encontrado." });
					}
					else
					{
						throw;
					}
				}
			}

			return Json(new { success = false, message = "Erro ao atualizar o plano de saúde." });
		}

		private async Task ProcessImageUploadAsync(PlanodeSaudeModel existingPlan, IFormFile imgUp)
		{
			// Remove imagem antiga se existir
			if (!string.IsNullOrEmpty(existingPlan.img))
			{
				string existingFilePath = Path.Combine(_caminho, "img", existingPlan.img);
				if (System.IO.File.Exists(existingFilePath))
				{
					System.IO.File.Delete(existingFilePath);
				}
			}

			// Define novo caminho e salva nova imagem
			string uploadsFolder = Path.Combine(_caminho, "img");
			Directory.CreateDirectory(uploadsFolder);

			string uniqueFileName = $"{Guid.NewGuid()}_{imgUp.FileName}";
			string filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await imgUp.CopyToAsync(fileStream);
			}

			existingPlan.img = uniqueFileName;
		}


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
				return Problem("Entity set 'SprintContext.PlanodeSaude' is null.");
			}

			var planodeSaudeModel = await _context.PlanodeSaude.FindAsync(id);

			if (planodeSaudeModel?.img != null && planodeSaudeModel.img.Length > 0)
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
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}


		private bool PlanodeSaudeModelExists(Guid id)
		{
			return (_context.PlanodeSaude?.Any(e => e.PlanodeSaudeId == id)).GetValueOrDefault();
		}

	}
}
