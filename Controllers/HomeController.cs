using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AsignacionBienesINEI.Models;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Services.IServices;

namespace AsignacionBienesINEI.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericService _genericService;       

        public HomeController(UserManager<ApplicationUser> userManager, IGenericService genericService)
        {
            _userManager = userManager;
            _genericService = genericService;           
        }

        [Authorize]
        public async Task<IActionResult> Index(int pg = 1, string nombre = null!)
        {
            IEnumerable<Bien> bienes;
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    bienes = await _genericService.GetAllEntity<Bien>();
                }
                else
                {
                    bienes = await _genericService.GetAllEntitiesByConditions<Bien>(c => c.Nombre!.Contains(nombre));
                }
                var ordered = bienes.OrderByDescending(b => b.Nombre);
                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                int recsCount = bienes.Count();
                var pager = new Pager(recsCount, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = ordered.Skip(recSkip).Take(pager.PageSize).ToList();
                ViewBag.Pager = pager;

                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.result = $"ERROR: {ex.Message}";
                return RedirectToAction("Error", "Home", new { msg = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult RegistrarBien()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> RegistrarBien(Bien bien)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _genericService.InsertEntity<Bien>(bien);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(bien);
                }
            }
            catch (Exception ex)
            {
                ViewBag.result = $"ERROR: {ex.InnerException?.Message}";
                return View(bien);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GestionarBien(int idBien)
        {
            try
            {
                if (idBien > 0)
                {
                    var e = await _genericService.GetEntity<Bien>(idBien);
                    return View(e);
                }
                else
                {
                    throw new Exception("El id debe ser mayor a 0");
                }
            }
            catch (Exception e)
            {
                ViewBag.result = $"ERROR: {e.InnerException?.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GestionarBien(Bien bien, string accion)
        {
            try
            {       
                if (ModelState.IsValid)
                {
                    var res = await _genericService.GetAndUpdateBien(bien, accion);
                    if (res > 0) 
                    { 
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View(bien);
                    }
                }
                else
                {
                    return View(bien);
                }
            }
            catch (Exception e)
            {
                ViewBag.result = $"ERROR: {e.Message}";
                return View(bien);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string msg)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = msg });
        }
    }
}