using Microsoft.EntityFrameworkCore;
using testeNav.Data;
using testeNav.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using testeNav.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<RoleService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar o Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false; // Mudar para true em produção
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Serviço de email (ajuste conforme necessário)
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Configurar serviços de sessão
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar MVC e Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configuração de tratamento de erros e exceção
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Função para inserir roles (pode ser movida para um DataSeeder separado)
async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Cliente", "Vendedor" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Verifica se a role "Vendedor" existe, se não existir, cria
    if (!await roleManager.RoleExistsAsync("Vendedor"))
    {
        await roleManager.CreateAsync(new IdentityRole("Vendedor"));
    }
}

// Chamar SeedRoles ao iniciar o app
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AssignRoleToUser(services, "exemplo@exemplo.com", "Vendedor");
}

 async Task AssignRoleToUser(IServiceProvider serviceProvider, string email, string roleName)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Verifica se o papel existe
    var roleExists = await roleManager.RoleExistsAsync(roleName);
    if (!roleExists)
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    var user = await userManager.FindByEmailAsync(email);
    if (user != null)
    {
        var isInRole = await userManager.IsInRoleAsync(user, roleName);
        if (!isInRole)
        {
            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configuração de rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
