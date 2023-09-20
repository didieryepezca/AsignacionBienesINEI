using AsignacionBienesINEI.BusinessLogic.ILogic;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;
using AsignacionBienesINEI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;

namespace AsignacionBienesINEI.Controllers
{
    public class AsignacionController : Controller
    {
        private readonly IAsignacionBL _asignacionBL;
        private readonly IGenericService _genericService;
        private readonly IAsignacionService _asignacionService;

        private static NuevaAsignacionViewModel _newAsignacionViewModel = new NuevaAsignacionViewModel();
        private static List<Bien>? _listaBienes = new List<Bien>();

        public AsignacionController(IAsignacionBL asignacionBL, IAsignacionService asignacionService, IGenericService genericService)
        {
            _asignacionBL = asignacionBL;
            _asignacionService = asignacionService;
            _genericService = genericService;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Lista(int estado, DateTime fecha, string accion)
        {
            IEnumerable<AsignacionDetalle> asignaciones;
            try
            {
                DateTime fechaS = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(accion))
                {
                    fechaS = fecha;
                }
                else
                {
                    fechaS = DateTime.Now;
                }
                ViewBag.fecha = fechaS.ToString("yyyy-MM-dd");

                if (fechaS.Date == Convert.ToDateTime("01/01/0001").Date)
                {
                    fechaS = DateTime.Now;
                }

                var estados = await _genericService.GetAllEntity<AsignacionEstado>();
                ViewBag.estados = estados;

                asignaciones = await _asignacionService.GetAllAsignacionesByDateAndStatus(estado, fechaS);
                return View(asignaciones);
            }
            catch (Exception ex)
            {
                ViewBag.result = $"ERROR: {ex.Message}";
                return RedirectToAction("Error", "Home", new { msg = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Nueva()
        {
            try
            {
                var responsables = await _genericService.GetAllEntity<ApplicationUser>();
                _newAsignacionViewModel.responsables = _asignacionBL.OnGetNuevaAsignacionResponsablesView(responsables.ToList());

                return View(_newAsignacionViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.result = $"ERROR: {ex.Message}";
                return RedirectToAction("Error", "Home", new { msg = ex.Message });
            }
        }

        public async Task<JsonResult> BuscarBienes(string tipo, string controlPatrimonial)
        {
            IEnumerable<Bien>? bienes;
            bienes = await _asignacionService.SearchBienPorControlPatrimonial(controlPatrimonial, tipo);
            _listaBienes = bienes.ToList();
            var results = bienes.Select(x => new
            {
                id = x.Id,
                label = x.Plaqueta,
            }).ToList();

            return Json(results);
        }

        public IActionResult OnSelectBienFromAutocomplete(int bienId) //Para refrescar la vista parcial de los bienes agregados.
        {
            try
            {
                var filteredBien = _listaBienes!.Where(b => b.Id == bienId).FirstOrDefault();
                if (filteredBien != null)
                {
                    _newAsignacionViewModel.asignacionesAgregadas = _asignacionBL.OnAddBienToAsignacionAgregadaView(filteredBien);
                }
                return PartialView("_AsignacionesAgregadas", _newAsignacionViewModel.asignacionesAgregadas);
            }
            catch (Exception ex)
            {
                //ViewBag.result = $"ERROR: {ex.Message}";
                return PartialView("_AsignacionesAgregadas", _newAsignacionViewModel.asignacionesAgregadas);
            }
        }

        public IActionResult OnRemoveBienFromAsignacionesAgregadas(int bienId)
        {
            var bienFromAsignacionesAgregadas = _newAsignacionViewModel.asignacionesAgregadas!.Where(b => b.Id == bienId).FirstOrDefault();
            if (bienFromAsignacionesAgregadas != null)
            {
                _newAsignacionViewModel.asignacionesAgregadas = _asignacionBL.OnRemoveBienFromAsignacionAgregadaView(bienFromAsignacionesAgregadas);
            }
            return PartialView("_AsignacionesAgregadas", _newAsignacionViewModel.asignacionesAgregadas);
        }

        public async Task<IActionResult> GetReporteAsignacionesPorUsuario(string userId)
        {
            _newAsignacionViewModel.asignacionesPorUsuario!.Clear();
            try
            {
                var asignacionesPorUsuario = await _asignacionService.GetAllAsignacionesPorUsuario(userId);
                var asignaciones = asignacionesPorUsuario.ToList();
                _newAsignacionViewModel.asignacionesPorUsuario = _asignacionBL.OnBuildAsignacionesPorUsuario(asignaciones);

                return PartialView("_RptAsignacionesUsuario", _newAsignacionViewModel.asignacionesPorUsuario);
            }
            catch (Exception ex)
            {
                ViewBag.result = $"ERROR: {ex.Message}";
                return PartialView("_RptAsignacionesUsuario", _newAsignacionViewModel.asignacionesPorUsuario);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Nueva(NuevaAsignacionViewModel nuevaAsignacionViewModel)
        {
            try
            {
                nuevaAsignacionViewModel.responsables = _newAsignacionViewModel.responsables;
                nuevaAsignacionViewModel.asignacionesAgregadas = _newAsignacionViewModel.asignacionesAgregadas;

                if (ModelState.IsValid)
                {
                    if (nuevaAsignacionViewModel.asignacionesAgregadas.Count == 0)
                    {
                        ViewBag.asignacionesVacias = "Debes asignar al menos un equipo";
                        return View(nuevaAsignacionViewModel);
                    }

                    //Llenamos el objeto _newAsignacionViewModel para utilizar los datos para llenar el PDF que se descargara via Javascript.
                    _newAsignacionViewModel.usuarioNombres = nuevaAsignacionViewModel.usuarioNombres;
                    _newAsignacionViewModel.usuarioDNI = nuevaAsignacionViewModel.usuarioDNI;
                    _newAsignacionViewModel.usuarioOdei = nuevaAsignacionViewModel.usuarioOdei;
                    _newAsignacionViewModel.observacion = nuevaAsignacionViewModel.observacion;

                    var result = await _asignacionService.CreateAsignacionWithDetails(nuevaAsignacionViewModel);
                    if (result.Item1 > 0)
                    {
                        _newAsignacionViewModel.idResultingFromDb = result.Item2;

                        var result2 = await _asignacionService.UpdateEquipmentStatus(nuevaAsignacionViewModel);

                        ViewBag.result = "Se ha realizado la asignación";
                        return View(nuevaAsignacionViewModel);
                    }
                    else
                    {
                        ViewBag.result = "Hubo un problema interno";
                        return View(nuevaAsignacionViewModel);
                    }
                }
                else
                {
                    return View(nuevaAsignacionViewModel);
                }
            }
            catch (Exception ex)
            {
                ViewBag.result = $"ERROR: {ex.Message} - Details: {ex.InnerException}";
                return View(nuevaAsignacionViewModel);
            }
        }

        public async Task<IActionResult> GenerateAsignacionPdfFileSinFirma(int idAsignacion)
        {
            try
            {
                if (idAsignacion > 0)//Este if solamente se ejecutará cuando se hará clic en el botón PDF de la lista de asignaciones.
                {
                    IEnumerable<AsignacionDetalle> asignaciones;
                    asignaciones = await _asignacionService.GetAsignacionPorId(idAsignacion);
                    var lista = asignaciones.ToList();

                    _newAsignacionViewModel.usuarioNombres = lista[0].Asignacion!.Responsable!.Nombres;
                    _newAsignacionViewModel.usuarioDNI = lista[0].Asignacion!.Responsable!.DNI;
                    _newAsignacionViewModel.usuarioOdei = lista[0].Asignacion!.Responsable!.Odei;
                    _newAsignacionViewModel.observacion = lista[0].Asignacion!.Observacion;

                    foreach (var asignacion in lista)
                    {
                        var filteredBien = await _genericService.GetEntity<Bien>(asignacion.IdBien);
                        if (filteredBien != null)
                        {
                            _newAsignacionViewModel.asignacionesAgregadas = _asignacionBL.OnAddBienToAsignacionAgregadaView(filteredBien);
                        }
                    }

                    _newAsignacionViewModel.idResultingFromDb = idAsignacion;
                }
                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.DisplayFooter = true;
                converter.Options.DisplayHeader = true;
                converter.Options.MarginTop = 10;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.DisplayHeader = false;
                converter.Options.DisplayFooter = false;
                converter.Options.CssMediaType = HtmlToPdfCssMediaType.Screen;
                converter.Options.EmbedFonts = true;

                string url = HttpContext.Request.Scheme + "://" + Request.Host + "/Asignacion/AsignacionPDF";

                PdfDocument doc = converter.ConvertUrl(url);

                // create memory stream to save PDF
                MemoryStream pdfStream = new MemoryStream();
                doc.Save(pdfStream);
                pdfStream.Seek(0, SeekOrigin.Begin);

                byte[] pdfByteArray = pdfStream.GetBuffer();

                //Limpiar lista para una nueva asignación
                _newAsignacionViewModel.asignacionesAgregadas.Clear(); //Limpiamos la lista por si queremos volver agregar otra.

                return File(pdfByteArray, "application/pdf", $"Asignacion-{_newAsignacionViewModel.idResultingFromDb}.pdf");
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema:" + ex.Message);
            }
        }

        public IActionResult AsignacionPDF()
        {
            return View(_newAsignacionViewModel);
        }

        public async Task<IActionResult> UploadAsignacionPDF(int idAsignacion)
        {
            var jres = new { msg = "" };
            try
            {
                var existingFile = await _genericService.GetAllEntitiesByConditions<AsignacionPDF>(f => f.IdAsignacion == idAsignacion);
                if (existingFile.Count() >= 1)
                {
                    jres = new { msg = "La asignacion ya cuenta con ficha vigente firmada." };
                    return Json(jres);
                }

                var files = Request.Form.Files;
                if (files.Count() == 0)
                {
                    jres = new { msg = "Debe seleccionar un archivo de asignacion en PDF. " };
                    return Json(jres);
                }

                foreach (IFormFile file in Request.Form.Files)
                {
                    if (!file.FileName.EndsWith(".pdf"))
                    {
                        jres = new { msg = "Su archivo no tiene la extensión PDF. " };
                        return Json(jres);
                    }

                    if (file.Length > 0)
                    {
                        var result = await _asignacionService.UploadSignedAsignationPDF(file, idAsignacion);
                    }
                    else
                    {
                        jres = new { msg = "El archivo no tiene ningun tamaño." };
                        return Json(jres);
                    }
                }
                jres = new { msg = "Archivo subido con exito." };
                return Json(jres);
            }
            catch (Exception ex)
            {
                jres = new { msg = ex.Message };
                return Json(jres);
            }
        }

        public async Task<IActionResult> DownloadPDFAsignacionFirmada(int idAsignacion)
        {
            try
            {
                byte[] pdfByteArray;
                var asignacionPDF = await _asignacionService.FindAsignacionFirmadaPDF(idAsignacion);
                pdfByteArray = (byte[])asignacionPDF.PdfData!;

                return File(pdfByteArray, "application/pdf", $"Asignacion-{idAsignacion}.pdf");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { msg = ex.Message });
            }
        }

        public async Task<IActionResult> ReleaseBienFromAsignacion(int idAsignacion, int idBien)
        {
            var jres = new { msg = "" };
            try
            {
                var result = await _asignacionService.UpdateAsignationAndReleaseEquipments(idAsignacion, idBien);
                if(result >  0) 
                {
                    jres = new { msg = "Se liberó el bien correctamente" };
                }
                else                 
                {
                    jres = new { msg = "Hubo un problema interno" };
                }                
                return Json(jres);
            }
            catch (Exception ex)
            {
                jres = new { msg = ex.Message };
                return Json(jres);
            }
        }
        public async Task<IActionResult> AsignacionView(int idAsignacion)
        {
            try
            {
                IEnumerable<AsignacionDetalle> asignaciones;
                asignaciones = await _asignacionService.GetAsignacionPorId(idAsignacion);
                var lista = asignaciones.ToList();

                _newAsignacionViewModel.usuarioNombres = lista[0].Asignacion!.Responsable!.Nombres;
                _newAsignacionViewModel.usuarioUserName = lista[0].Asignacion!.Responsable!.UserName;
                _newAsignacionViewModel.usuarioDNI = lista[0].Asignacion!.Responsable!.DNI;
                _newAsignacionViewModel.usuarioOdei = lista[0].Asignacion!.Responsable!.Odei;

                _newAsignacionViewModel.equipoFueraINEI = lista[0].Asignacion!.EquipoFueraINEI!;
                _newAsignacionViewModel.equipoLibre = lista[0].Asignacion!.EquipoLibre!;

                _newAsignacionViewModel.observacion = lista[0].Asignacion!.Observacion;

                foreach (var asignacion in lista)
                {
                    var filteredBien = await _genericService.GetEntity<Bien>(asignacion.IdBien);
                    if (filteredBien != null)
                    {
                        _newAsignacionViewModel.asignacionesAgregadas = _asignacionBL.OnAddBienToAsignacionAgregadaView(filteredBien);
                    }
                }

                var asignacionesPorUsuario = await _asignacionService.GetAllAsignacionesPorUsuario(lista[0].Asignacion!.Responsable!.Id);
                var asignacionPorUsuario = asignacionesPorUsuario.ToList();
                _newAsignacionViewModel.asignacionesPorUsuario = _asignacionBL.OnBuildAsignacionesPorUsuario(asignacionPorUsuario);

                return View(_newAsignacionViewModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema:" + ex.Message);
            }
        }
    }
}