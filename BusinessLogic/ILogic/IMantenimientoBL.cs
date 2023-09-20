using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;

namespace AsignacionBienesINEI.BusinessLogic.ILogic
{
    public interface IMantenimientoBL
    {
        Mantenimiento OnPostCreateMantenimiento(NuevoMantenimientoViewModel mtto);
    }
}