using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class UsuariosController: Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsuariosController(UserManager<IdentityUser>userManager,
            SignInManager<IdentityUser>signInManager) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>Registro (RegistroViewModel modelo)
        {
            if(!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>Login(LoginViewModel modelo) 
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email,
                modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);//lockoutOnFailure: false significa que si el 
            if (resultado.Succeeded)                                          // usuario se confunde en muchos intentos se bloquea la cuenta
            {
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Email o password incorrecta");
                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
