using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using AsignacionBienesINEI.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AsignacionBienesINEI.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Ingrese al menos un Nombre")]
            [Display(Name = "Nombres")]
            public string Nombres { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese su Apellido Paterno")]
            [Display(Name = "Apellido Paterno")]
            public string apellidoPaterno { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese su Apellido Materno")]
            [Display(Name = "Apellido Materno")]
            public string apellidoMaterno { get; set; } = String.Empty;

            [StringLength(100, ErrorMessage = "El DNI Debe contener una longitud de 8 dígitos", MinimumLength = 8)]
            [Display(Name = "DNI")]
            public string DNI { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese su Condición Laboral")]
            [Display(Name = "Condicion Laboral")]
            public string condicionLaboral { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese su Fecha de Nacimiento dd/mm/AAAA")]
            [Display(Name = "Fecha de Nacimiento")]
            public string fechaNacimiento { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese su Profesión/Estudio")]
            [Display(Name = "Profesion/Estudio")]
            public string profesionEstudio { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese su Grado Académico")]
            [Display(Name = "Grado Académico")]
            public string gradoAcadémico { get; set; } = String.Empty;

            [Display(Name = "Teléfono/Celular")]
            public string telefonoCelular { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese su Email")]
            [EmailAddress]
            [Display(Name = "Email")]
            [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "El correo ingresado no es válido")]
            public string Email { get; set; } = String.Empty;

            [Required(ErrorMessage = "Ingrese una Contraseña")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = String.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Required(ErrorMessage = "Debe ingresar la misma contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y su confirmación no coinciden")]
            public string ConfirmPassword { get; set; } = String.Empty;
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //var user = CreateUser();
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    DNI = Input.DNI,
                    Nombres = Input.Nombres,
                    ApellidoPaterno = Input.apellidoPaterno,
                    ApellidoMaterno = Input.apellidoMaterno,
                    FechaRegistro = DateTime.Now,
                    FechaNacimiento = Input.fechaNacimiento,
                    CondicionLaboral = Input.condicionLaboral,
                    Odei = "CAJAMARCA",
                    GradoAcademico = Input.gradoAcadémico,
                    ProfesionEstudio = Input.profesionEstudio,
                    TelefonoCelular = Input.telefonoCelular
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var roles = await _roleManager.Roles.ToListAsync();
                    if (roles.Count == 0)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Enums.Roles.Administrador.ToString()));
                    }
                    await _userManager.AddToRoleAsync(user, Enums.Roles.Administrador.ToString());
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
