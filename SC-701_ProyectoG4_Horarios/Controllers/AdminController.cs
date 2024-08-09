using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC_701_ProyectoG4_Horarios.DAL;
using SC_701_ProyectoG4_Horarios.Models;

namespace SC_701_ProyectoG4_Horarios.Controllers
{
    public class AdminController : Controller
    {
        private readonly AuthDbContext _authDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<Usuario> _userStore;
        private readonly IUserEmailStore<Usuario> _emailStore;
        private readonly UserManager<Usuario> _userManager;

        public AdminController(AuthDbContext authDbContext, RoleManager<IdentityRole> roleManager,
            IUserStore<Usuario> userStore, UserManager<Usuario> userManager)
        {
            _authDbContext = authDbContext;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<Usuario>)_userStore;
            _userManager = userManager;

        }

        [Authorize(Roles = "Admin")]
        public IActionResult CrearUsuario()
        {
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearUsuario(AdminCrearUsuarioViewModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario();

                await _userStore.SetUserNameAsync(user, usuarioModel.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, usuarioModel.Email, CancellationToken.None);
                user.Nombre = usuarioModel.Nombre;
                user.PrimerApellido = usuarioModel.PrimerApellido;
                user.SegundoApellido = usuarioModel.SegundoApellido;
                var result = await _userManager.CreateAsync(user, usuarioModel.Password);

                if (result.Succeeded)
                {
                   
                    //Se agrega el rol 
                    string normalizeRoleName = _roleManager.Roles.FirstOrDefault(r => r.Id 
                    == usuarioModel.IdRol).NormalizedName;
                    var resultRole = await _userManager.AddToRoleAsync(user, normalizeRoleName);
                    
                    return RedirectToAction("Index",  "Home");
                }
            }
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            return View();
        }
    }
}
