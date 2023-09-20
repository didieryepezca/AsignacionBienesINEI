using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;

namespace AsignacionBienesINEI.BusinessLogic.ILogic
{
    public interface IAsignacionBL
    {
        List<ResponsablesViewModel> OnGetNuevaAsignacionResponsablesView(List<ApplicationUser> userList);

        List<AsignacionesAgregadasViewModel> OnAddBienToAsignacionAgregadaView(Bien filteredBien);

        List<AsignacionesAgregadasViewModel> OnRemoveBienFromAsignacionAgregadaView(AsignacionesAgregadasViewModel bienFromAsignacionesAgregadas);        

        List<AsignacionesPorUsuarioViewModel> OnBuildAsignacionesPorUsuario(List<AsignacionDetalle> asignaciones);
    }
}