using AsignacionBienesINEI.BusinessLogic.ILogic;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;

namespace AsignacionBienesINEI.BusinessLogic.Logic
{
    public class AsignacionBL : IAsignacionBL
    {
        public static List<ResponsablesViewModel> _responsablesList = new List<ResponsablesViewModel>();
        private static List<AsignacionesAgregadasViewModel> _asignacionesAgregadas = new List<AsignacionesAgregadasViewModel>();
        private static List<AsignacionesPorUsuarioViewModel> _asignacionesPorUsuario = new List<AsignacionesPorUsuarioViewModel>();

        public List<ResponsablesViewModel> OnGetNuevaAsignacionResponsablesView(List<ApplicationUser> userList)
        {
            foreach (ApplicationUser usuario in userList)
            {
                var responsable = new ResponsablesViewModel()
                {
                    Id = usuario.Id,
                    DNI = usuario.DNI,
                    Nombres = usuario.Nombres,
                    UserName = usuario.UserName,
                    ODEI = usuario.Odei,
                };
                _responsablesList.Add(responsable);
            }
            return _responsablesList;
        }

        public List<AsignacionesAgregadasViewModel> OnAddBienToAsignacionAgregadaView(Bien filteredBien)
        {
            //Comprobar que el mismo bien no se agregue 2 veces.
            var existingBien = _asignacionesAgregadas.Where(a => a.Id == filteredBien.Id).FirstOrDefault();
            if (existingBien == null)
            {
                var bien = new AsignacionesAgregadasViewModel()
                {
                    Id = filteredBien.Id,
                    CP = filteredBien.Plaqueta,
                    Marca = filteredBien.Marca!,
                    Modelo = filteredBien.Modelo!,
                    DireccionTecnica = "CAJAMARCA",
                    //FechaIngreso = DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
                    FechaIngreso  = DateTime.Now
                };
                _asignacionesAgregadas.Add(bien);
            }
            return _asignacionesAgregadas;
        }

        public List<AsignacionesAgregadasViewModel> OnRemoveBienFromAsignacionAgregadaView(AsignacionesAgregadasViewModel bienFromAsignacionesAgregadas)
        {
            _asignacionesAgregadas.Remove(_asignacionesAgregadas.Single(a => a.Id == bienFromAsignacionesAgregadas.Id));
            return _asignacionesAgregadas;
        }
      
        public List<AsignacionesPorUsuarioViewModel> OnBuildAsignacionesPorUsuario(List<AsignacionDetalle> asignaciones) 
        {
            foreach (var det in asignaciones) 
            {
                var item = new AsignacionesPorUsuarioViewModel()
                {
                    Equipo = det.Bien!.Nombre,
                    Marca = det.Bien.Marca!,
                    Modelo = det.Bien.Modelo!,
                    CP = det.Bien.Plaqueta,
                    DireccionTecnica = "CAJAMARCA",
                    Fecha = det.FechaIngreso.ToString()
                };
                _asignacionesPorUsuario.Add(item);
            }
            return _asignacionesPorUsuario;
        }
    }
}