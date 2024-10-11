using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace testeNav.Controllers
{
    public class CarrinhoController : Controller
    {
        // GET: CarrinhoController
        public ActionResult Carrinho()
        {
            return View();
        }

        // GET: CarrinhoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarrinhoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarrinhoController/Create
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

        // GET: CarrinhoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CarrinhoController/Edit/5
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

        // GET: CarrinhoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CarrinhoController/Delete/5
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
