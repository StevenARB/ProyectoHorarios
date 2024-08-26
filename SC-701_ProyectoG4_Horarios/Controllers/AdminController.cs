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
        public async Task<ActionResult> Index()
        {

            var usuarios = await _userManager.Users.ToListAsync();
            ViewBag.usuarios = usuarios;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetallesUsuario(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CrearUsuario()
        {
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
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
                    
                    return RedirectToAction("Index", "Admin");
                }
            }
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var rolesUser = await _userManager.GetRolesAsync(user);
            var roleId = _roleManager.Roles.FirstOrDefault(r => r.NormalizedName == rolesUser.FirstOrDefault())?.Id;

            var model = new AdminEditarUsuarioViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.Nombre,
                PrimerApellido = user.PrimerApellido,
                SegundoApellido = user.SegundoApellido,
                IdRol = roleId
            };
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name", model.IdRol);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(AdminEditarUsuarioViewModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(usuarioModel.Id);

                if (user == null)
                {
                    return NotFound(); // Manejar el caso donde no se encuentra el usuario
                }

                user.Nombre = usuarioModel.Nombre;
                user.PrimerApellido = usuarioModel.PrimerApellido;
                user.SegundoApellido = usuarioModel.SegundoApellido;

                // Actualizar el nombre de usuario y correo electrónico si es necesario
                var currentUserName = await _userStore.GetUserNameAsync(user, CancellationToken.None);
                if (currentUserName != usuarioModel.Email)
                {
                    await _userStore.SetUserNameAsync(user, usuarioModel.Email, CancellationToken.None);
                }

                var currentEmail = await _emailStore.GetEmailAsync(user, CancellationToken.None);
                if (currentEmail != usuarioModel.Email)
                {
                    await _emailStore.SetEmailAsync(user, usuarioModel.Email, CancellationToken.None);
                }

                if (usuarioModel.OldPassword != null && usuarioModel.Password == usuarioModel.ConfirmPassword)
                {
                    var changePassword = await _userManager.ChangePasswordAsync(user, usuarioModel.OldPassword, usuarioModel.Password);
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Actualizar el rol del usuario si ha cambiado
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var newRoleName = _roleManager.Roles.FirstOrDefault(r => r.Id == usuarioModel.IdRol)?.Name;

                    if (newRoleName != null && !currentRoles.Contains(newRoleName))
                    {
                        // Remover los roles actuales
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);

                        // Agregar el nuevo rol
                        await _userManager.AddToRoleAsync(user, newRoleName);
                    }

                    return RedirectToAction("Index", "Admin");
                }
            }

            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            return View(usuarioModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EliminarUsuario(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("EliminarUsuario")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarUsuarioConfirmado(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            return RedirectToAction("Index", "Admin");
        }
    }
}
