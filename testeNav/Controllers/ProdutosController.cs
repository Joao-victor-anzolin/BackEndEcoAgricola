using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using testeNav.Data;
using testeNav.Models;

namespace testeNav.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        // Construtor unificado para injetar o UserManager e o ApplicationDbContext
        public ProdutosController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Perfil do usuário, verificando se é vendedor ou cliente
        [Authorize]  // Garante que o usuário esteja autenticado
        public async Task<IActionResult> Perfil()
        {
            // Obtém o usuário atual
            var user = await _userManager.GetUserAsync(User);

            // Verifica se o usuário tem o role de "Vendedor"
            if (user != null && await _userManager.IsInRoleAsync(user, "Vendedor"))
            {
                // Redireciona para a view específica do vendedor
                return View("Index");  
            }

            // Se o usuário for um "Cliente", redireciona para a view específica do cliente
            return View("Index");  // Certifique-se de que a view "HomeCliente" exista
        }

        public async Task<IActionResult> Index()
        {
            // Recupera todos os produtos do banco de dados
            var produtos = await _context.Produtos.ToListAsync();

            // Verifica se a lista de produtos está nula
            if (produtos == null || !produtos.Any())
            {
                // Inicializa uma lista vazia se não houver produtos
                produtos = new List<ProdutoModel>();
            }

            // Passa a lista de produtos para a view
            return View(produtos);
        }


        // GET: ProdutosController
        public IActionResult Produtos()
        {
            return View();
        }

        // GET: ProdutosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProdutosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProdutosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProdutosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProdutosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProdutosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProdutosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}