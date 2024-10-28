using Microsoft.AspNetCore.Identity;
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
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;

		public ConsultasModelsController(SprintContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
			_signInManager = signInManager;
			_userManager = userManager;
		}

        // GET: ConsultasModels
        public async Task<IActionResult> Index(Guid? id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);

                // Verifica se o usuário está na role "Admin"
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    // Exibe todas as consultas para o admin
                    var consultasContext = _context.Consultas
                        .Include(c => c.Consultorio)
                        .Include(c => c.Pacientes);
                    return View(await consultasContext.ToListAsync());
                }
                else
                {
                    // Verifica se o paciente está associado ao usuário logado
                    var paciente = await _context.Paciente
                        .FirstOrDefaultAsync(p => p.UserId == user.Id);

                    if (paciente != null)
                    {
                        // Verifica se o paciente já tem consultas associadas
                        var consultaExistente = await _context.Consultas
                            .Include(c => c.Consultorio)
                            .FirstOrDefaultAsync(c => c.PacienteId == paciente.PacienteId);

                        if (consultaExistente != null)
                        {
                            // Se já existe uma consulta, exibe os detalhes dessa consulta
                            return RedirectToAction("Details", new { id = consultaExistente.ConsultaId });
                        }
                        else
                        {
                            // Caso não tenha consulta, redireciona para a criação de uma nova consulta
                            return Redirect("/ConsultasModels/Create");
                        }
                    }
                    else
                    {
                        // Se não existe paciente, redireciona para criar um paciente
                        return Redirect("/PacientesModels/Create");
                    }
                }
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }

        // GET: ConsultasModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

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
        // GET: ConsultasModels/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redireciona para a página de login se o usuário não estiver autenticado
            }

            // Exibe todos os consultórios disponíveis
            ViewBag.Consultorio = new SelectList(_context.Consultorios, "ConsultorioId", "Nome");

            // Filtra o paciente vinculado ao usuário autenticado
            var paciente = await _context.Paciente
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            // Verifica se encontrou um paciente vinculado ao usuário
            if (paciente == null || !paciente.Any())
            {
                ModelState.AddModelError("", "Paciente não encontrado.");
                return View(); // Opcionalmente, você pode redirecionar para outra página ou exibir uma mensagem
            }

            // Preenche o ViewBag com o paciente do usuário autenticado
            ViewBag.Pacientes = new SelectList(paciente, "PacienteId", "Nome");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsultaId,DataConsultas,Hora,ConsultorioId")] ConsultasModel consultasModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "Usuário não autenticado");
                return View(consultasModel);
            }
            // Verifica o usuário logado
            if (user == null)
            {
                ModelState.AddModelError("", "Usuário não autenticado");
                return View(consultasModel);
            }

            // Verificar se o paciente está associado ao usuário logado
            var paciente = await _context.Paciente.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (paciente == null)
            {
                ModelState.AddModelError("", "Paciente não encontrado.");
                return View(consultasModel);
            }

            // Atribuir o ID do paciente ao modelo de consulta
            consultasModel.UserId = user.Id;
            consultasModel.PacienteId = paciente.PacienteId;
            consultasModel.ConsultaId = Guid.NewGuid(); // Gera um novo ID para a consulta

            // Verifica se o modelo de dados está válido
            if (ModelState.IsValid)
            {
                try
                {
                    // Tentar adicionar a consulta no banco de dados
                    _context.Add(consultasModel);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Consulta salva com sucesso."); // Log para verificar se salvou

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Erro ao salvar no banco de dados: {ex.Message}");
                    ModelState.AddModelError("", "Erro ao salvar no banco de dados.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro inesperado: {ex.Message}");
                    ModelState.AddModelError("", "Ocorreu um erro inesperado.");
                }
            }
            else
            {
                // Log dos erros de validação
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Erro de validação: {error.ErrorMessage}");
                    }
                }
            }
            Console.WriteLine("Usuário autenticado: " + user.Id);
            Console.WriteLine("Paciente associado: " + paciente.PacienteId);
            // Recarregar os dados do consultório
            ViewData["ConsultorioId"] = new SelectList(_context.Consultorios, "ConsultorioId", "Nome", consultasModel.ConsultorioId);
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
            ViewBag.Consultorios = new SelectList(_context.Consultorios, "ConsultorioId", "Nome");
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
