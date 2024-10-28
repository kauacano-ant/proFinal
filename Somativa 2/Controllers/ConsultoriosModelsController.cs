using Microsoft.AspNetCore.Identity;
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
        private string _caminho;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;



		public ConsultoriosModelsController(SprintContext context, IWebHostEnvironment hostEnvironment, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _caminho = hostEnvironment.WebRootPath;
			_signInManager = signInManager;
            _userManager = userManager;
		}

        public async Task<IActionResult> Consultorios()
        {
            var Consultorios = await _context.Categ.Include(c => c.Nome).ToListAsync();
            ViewData["Categ"] = await _context.Categ.ToListAsync();

            return View(Consultorios);
        }

        public async Task<IActionResult> SearchProd(Guid inCategoria, string inNome)
        {
            var prod = await _context.Consultorios.Include(c => c.Categ).ToListAsync();

            if (inNome != null)
            {
                inNome = inNome.Trim().ToUpper();
                prod = prod.Where(i => i.Nome.ToUpper().Contains(inNome)).ToList();
            }

            if (!inCategoria.ToString().Equals("00000000-0000-0000-0000-000000000000"))
            {
                prod = prod.Where(i => i.CategId == inCategoria).ToList();
            }

            ViewData["Categ"] = await _context.Categ.ToListAsync();
            return View("Index", prod);
        }

        // GET: ConsultoriosModels
        public async Task<IActionResult> Index()
        {
            try
            {
                if (_context.Consultorios == null)
                {
                    return Problem("Entity set 'SprintContext.Consultorios' is null.");
                }

                var consultoriosList = await _context.Consultorios
                    .Include(c => c.Categ)
                    .ToListAsync();

                var categs = await _context.Categ.ToListAsync();

                ViewData["Categ"] = categs;

                return View(consultoriosList);
            }
            catch (Exception ex)
            {
                // Log the exception
                return Problem($"An error occurred: {ex.Message}");
            }
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

			if (_signInManager.IsSignedIn(User))
			{
				var user = await _userManager.GetUserAsync(User); // Obtém o usuário atual

				// Verifica se o usuário está na role "Admin"
				if (await _userManager.IsInRoleAsync(user, "Admin"))
				{
					return View(consultoriosModel);
				}
				else
				{
					return Redirect("/PacientesModels/Create");
				}
			}
			else
			{
				return Redirect("/Identity/Account/Login");
			}

		}

        // GET: ConsultoriosModels/Create
        public IActionResult Create()
        {
            ViewData["CategId"] = new SelectList(_context.Categ, "CategId", "Nome");
            return View();
        }

        // POST: ConsultoriosModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsultorioId,Nome,Endereco,Telefone,Especialidade,Email,img,CategId,Descricao")] ConsultoriosModel consultoriosModel, IFormFile imgUp)
        {
            consultoriosModel.ConsultorioId = Guid.NewGuid();
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
                consultoriosModel.img = uniqueFileName;
            }
            if (ModelState.IsValid)
            {
                _context.Add(consultoriosModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategId"] = new SelectList(_context.Categ, "CategId", "Nome", consultoriosModel.CategId);
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
            ViewData["CategId"] = new SelectList(_context.Categ, "CategId", "Nome", consultoriosModel.CategId);

            return View(consultoriosModel);

        }

        // POST: ConsultoriosModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ConsultorioId,Nome,Endereco,Telefone,Especialidade,Email,CategId,img,Descricao")] ConsultoriosModel consultoriosModel, IFormFile imgUp)
        {
            if (id != consultoriosModel.ConsultorioId)
            {
                return NotFound();
            }

            // Verifica se já existe um consultório com o ID fornecido
            var consultoriosModel2 = await _context.Consultorios.FindAsync(id);
            if (consultoriosModel2 == null)
            {
                return NotFound();
            }

            // Define o caminho da pasta onde a imagem será salva (ex: wwwroot/img)
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");

            // Se uma nova imagem foi carregada, excluir a antiga e salvar a nova
            if (imgUp != null && imgUp.Length > 0)
            {
                // Se houver uma imagem existente, excluí-la
                if (!string.IsNullOrEmpty(consultoriosModel2.img))
                {
                    string existingFilePath = Path.Combine(uploadsFolder, consultoriosModel2.img);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }

                // Salvar a nova imagem
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

                consultoriosModel2.img = uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza os outros campos
                    consultoriosModel2.Nome = consultoriosModel.Nome;
                    consultoriosModel2.Endereco = consultoriosModel.Endereco;
                    consultoriosModel2.Telefone = consultoriosModel.Telefone;
                    consultoriosModel2.Especialidade = consultoriosModel.Especialidade;
                    consultoriosModel2.Email = consultoriosModel.Email;
                    consultoriosModel2.CategId = consultoriosModel.CategId;
                    consultoriosModel2.Descricao = consultoriosModel.Descricao;

                    // Apenas uma chamada para Update
                    _context.Update(consultoriosModel2);
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

            ViewData["CategId"] = new SelectList(_context.Categ, "CategId", "Nome", consultoriosModel.CategId);
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
                .Include(c => c.Categ)
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
            if (consultoriosModel.img != null && consultoriosModel.img.Length > 0)
            {
                string uploadsFolder = Path.Combine(_caminho, "img");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, consultoriosModel.img);

                  
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
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
