using AsignacionBienesINEI.BusinessLogic.ILogic;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;
using AsignacionBienesINEI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AsignacionBienesINEI.Controllers
{
    public class MantenimientoController : Controller
    {
        private readonly IGenericService _genericService;        
        private readonly IAsignacionBL _asignacionBL;
        private readonly IMantenimientoBL _mantenimientoBL;

        private static NuevoMantenimientoViewModel _nvoMantenimientoViewModel = new NuevoMantenimientoViewModel();

        public MantenimientoController(IGenericService genericService, IAsignacionBL asignacionBL, IMantenimientoBL mantenimientoBL)
        {
            _genericService = genericService;            
            _asignacionBL = asignacionBL;
            _mantenimientoBL = mantenimientoBL;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Mantenimiento> mantenimientos;
            try 
            {
                mantenimientos = await _genericService.GetAllEntity<Mantenimiento>();
                return View(mantenimientos);
            }
            catch (Exception ex) 
            {
                return RedirectToAction("Error", "Home", new { msg = ex.Message });
            }            
        }

        public IActionResult Nuevo()
        {
            _nvoMantenimientoViewModel.Odei = "CAJAMARCA";
            return View(_nvoMantenimientoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(NuevoMantenimientoViewModel nvoMantenimientoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_nvoMantenimientoViewModel.equipoAgregado.Count == 0)
                    {
                        ViewBag.BienVacio = "Debes seleccionar un bien";
                        return View(nvoMantenimientoViewModel);
                    }

                    nvoMantenimientoViewModel.equipoAgregado = _nvoMantenimientoViewModel.equipoAgregado;

                    var mtto = _mantenimientoBL.OnPostCreateMantenimiento(nvoMantenimientoViewModel);
                    var resultId = await _genericService.InsertEntity<Mantenimiento>(mtto);

                    //_nvoMantenimientoViewModel.idResultingFromDb = resultId;
                    ViewBag.result = $"Se ha registrado el mantenimiento";
                    return View(nvoMantenimientoViewModel);
                }
                else 
                {
                    return View(nvoMantenimientoViewModel);
                }               
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { msg = ex.Message });
            }
        }

        public async Task<IActionResult> OnSelectBienFromAutocomplete(int bienId) //Para refrescar la vista parcial de los bienes agregados.
        {
            try
            {
                var filteredBien = await _genericService.GetAllEntitiesByConditions<Bien>(b => b.Id == bienId);
                if (filteredBien != null)
                {
                    var bien = filteredBien.FirstOrDefault();

                    _nvoMantenimientoViewModel.equipoAgregado.Clear();
                    _nvoMantenimientoViewModel.equipoAgregado = _asignacionBL.OnAddBienToAsignacionAgregadaView(bien!);
                }
                return PartialView("_EquipoAgregado", _nvoMantenimientoViewModel.equipoAgregado);
            }
            catch (Exception ex)
            {
                ViewBag.result = $"ERROR: {ex.Message}";
                return PartialView("_EquipoAgregado", _nvoMantenimientoViewModel.equipoAgregado);
            }
        }
    }
}