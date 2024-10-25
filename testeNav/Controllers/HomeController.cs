using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testeNav.Models;

namespace testeNav.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            
            var user = await _userManager.GetUserAsync(User);

            
            if (user != null && await _userManager.IsInRoleAsync(user, "Vendedor"))
            {
               
                ViewBag.foto = user.foto;
            }
            else
            {
               
                ViewBag.foto = null;
            }

            return View();
        }
    }
}
