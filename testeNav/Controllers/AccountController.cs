using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testeNav.Models;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // Método para listar todos os usuários e suas roles
    public async Task<IActionResult> ListUsersWithRoles()
    {
        var users = _userManager.Users.ToList(); // Pega todos os usuários
        var userRoles = new List<UserRolesViewModel>(); // Cria uma lista para armazenar os usuários e suas roles

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user); // Pega as roles do usuário
            userRoles.Add(new UserRolesViewModel
            {
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        return View(userRoles); // Retorna a view com a lista de usuários e roles
    }
}

// ViewModel para armazenar a informação de usuários e suas roles
public class UserRolesViewModel
{
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}