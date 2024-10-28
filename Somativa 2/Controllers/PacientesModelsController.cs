using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
		private readonly SignInManager<IdentityUser> _signInManager;
		private string _caminho;
		private readonly UserManager<IdentityUser> _userManager;

		public PacientesModelsController(SprintContext context, SignInManager<IdentityUser> signInManager, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_caminho = hostEnvironment.WebRootPath;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		// GET: PacientesModels
		public async Task<IActionResult> Index(Guid? id)
		{
			if (_signInManager.IsSignedIn(User))
			{
				var user = await _userManager.GetUserAsync(User); // Obtém o usuário atual

				// Verifica se o usuário está na role "Admin"
				if (await _userManager.IsInRoleAsync(user, "Admin"))
				{
					var sprintContext = _context.Paciente.Include(p => p.PlanodeSaude);
					return View(await sprintContext.ToListAsync());
				}
				else
				{
					var pacientesModel = await _context.Paciente
					.Include(p => p.PlanodeSaude)
					.FirstOrDefaultAsync(p => p.UserId == user.Id);
					if (pacientesModel != null)
					{
						return RedirectToAction("Details", new { id = pacientesModel.PacienteId });
					}
					else
					{
						return Redirect("/PacientesModels/Create");
					}
				}
			}
			else
			{
				return Redirect("/Identity/Account/Login");
			}
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
			ViewBag.PlanodeSaude = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "Nome");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Nome,CPF,RG,Data_de_nascimento,Endereco,Telefone,PlanodeSaudeId")] PacientesModel pacientesModel, IFormFile imgUp)
		{
			// Obtém o usuário atual
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				ModelState.AddModelError("", "Usuário não autenticado");
				return View(pacientesModel);
			}

			// Atribui o ID do usuário ao paciente
			pacientesModel.UserId = user.Id;
			pacientesModel.PacienteId = Guid.NewGuid(); // Gera um novo PacienteId

			// Lógica para o upload da imagem
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
				pacientesModel.img = uniqueFileName; // Salva o nome do arquivo no modelo
			}

			// Verifica se o modelo é válido
			if (ModelState.IsValid)
			{
				try
				{
					_context.Add(pacientesModel); // Adiciona o paciente ao contexto
					await _context.SaveChangesAsync(); // Salva no banco
					return RedirectToAction(nameof(Index)); // Redireciona após salvar
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Erro ao salvar paciente: {ex.Message}");
					ModelState.AddModelError("", "Erro ao salvar dados. Tente novamente.");
				}
			}
			else
			{
				// Log dos erros de validação
				foreach (var modelState in ModelState.Values)
				{
					foreach (var error in modelState.Errors)
					{
						Console.WriteLine(error.ErrorMessage);
					}
				}
			}

			Console.WriteLine("Id do usuário: " + user.Id);
			// Se a criação falhar, manter os planos de saúde na ViewBag
			ViewBag.PlanodeSaude = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "Nome", pacientesModel.PlanodeSaudeId);
			return View(pacientesModel); // Retorna para a página de criação se houver erro
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
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [Bind("PacienteId,Nome,CPF,RG,Data_de_nascimento,Endereco,Telefone,PlanodeSaudeId")] PacientesModel pacientesModel, IFormFile imgUp)
		{
			if (id != pacientesModel.PacienteId)
			{
				return NotFound();
			}

			var pacientesModel2 = await _context.Paciente.FindAsync(id);
			if (pacientesModel2 == null)
			{
				return NotFound();
			}

			if (imgUp != null && imgUp.Length > 0)
			{
				if (!string.IsNullOrEmpty(pacientesModel2.img))
				{
					string existingFilePath = Path.Combine(_caminho, "img", pacientesModel2.img);
					if (System.IO.File.Exists(existingFilePath))
					{
						System.IO.File.Delete(existingFilePath);
					}
				}

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

				pacientesModel2.img = uniqueFileName;
			}

			if (ModelState.IsValid)
			{
				try
				{
					pacientesModel2.Nome = pacientesModel.Nome;
					pacientesModel2.CPF = pacientesModel.CPF;
					pacientesModel2.RG = pacientesModel.RG;
					pacientesModel2.Data_de_nascimento = pacientesModel.Data_de_nascimento;
					pacientesModel2.Endereco = pacientesModel.Endereco;
					pacientesModel2.Telefone = pacientesModel.Telefone;
					pacientesModel2.PlanodeSaudeId = pacientesModel.PlanodeSaudeId;

					_context.Update(pacientesModel2);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PacientesModelExists(pacientesModel2.PacienteId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}

			ViewData["PlanodeSaudeId"] = new SelectList(_context.PlanodeSaude, "PlanodeSaudeId", "PlanodeSaudeId", pacientesModel.PlanodeSaudeId);
			return View(pacientesModel2);
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