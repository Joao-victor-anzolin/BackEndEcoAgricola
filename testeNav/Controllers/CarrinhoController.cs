using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testeNav.Models;
using testeNav.Data;
using testeNav.Extensoes;

namespace testeNav.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarrinhoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Exibe o carrinho
        public IActionResult Index()
        {
            var carrinho = ObterCarrinho();
            return View(carrinho);
        }

        // Adiciona item ao carrinho
        public IActionResult AdicionarAoCarrinho(int Id)
        {
            var produto = _context.Produtos.Find(Id);
            if (produto == null)
            {
                return NotFound();
            }

            var carrinho = ObterCarrinho();

            // Aqui você pode definir a quantidade, por exemplo, 1
            int quantidade = 1;
            carrinho.AdicionarItem(produto, quantidade);  // Certifique-se de passar os argumentos corretos
            SalvarCarrinho(carrinho);

            return RedirectToAction("Index", "Carrinho");
        }

        // Remove item do carrinho
        public IActionResult RemoverDoCarrinho(int produtoId)
        {
            var carrinho = ObterCarrinho();
            var itemExistente = carrinho.Itens.FirstOrDefault(i => i.Produto.Id == produtoId);

            if (itemExistente != null)
            {
                carrinho.Itens.Remove(itemExistente);
                SalvarCarrinho(carrinho);
            }

            return RedirectToAction("Index");
        }

        // Métodos auxiliares para o carrinho na sessão
        private CarrinhoModel ObterCarrinho()
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<CarrinhoModel>("Carrinho") ?? new CarrinhoModel();
            return carrinho;
        }

        private void SalvarCarrinho(CarrinhoModel carrinho)
        {
            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
        }

        public IActionResult Checkout()
        {
            var carrinho = ObterCarrinho();  // Obtenha o carrinho da sessão
            if (carrinho.Itens.Count == 0)
            {
                ModelState.AddModelError("", "O carrinho está vazio.");
                return RedirectToAction("Index");
            }

            var pedido = new Pedido
            {
                UsuarioId = User.Identity.Name,
                DataPedido = DateTime.Now,
                Total = carrinho.Itens.Sum(item => item.Produto.Preco * item.Quantidade),
                Itens = new List<ItemPedido>()
            };

            foreach (var itemCarrinho in carrinho.Itens)
            {
                var itemPedido = new ItemPedido
                {
                    ProdutoId = itemCarrinho.ProdutoId,
                    Produto = itemCarrinho.Produto,
                    Quantidade = itemCarrinho.Quantidade,
                    PrecoUnitario = itemCarrinho.Produto.Preco
                };
                pedido.Itens.Add(itemPedido);
            }


            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            HttpContext.Session.Remove("Carrinho");

            return RedirectToAction("Confirmacao");
        }
    }
}
