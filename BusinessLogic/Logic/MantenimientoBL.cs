using AsignacionBienesINEI.BusinessLogic.ILogic;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;

namespace AsignacionBienesINEI.BusinessLogic.Logic
{
    public class MantenimientoBL : IMantenimientoBL
    {
        public Mantenimiento OnPostCreateMantenimiento(NuevoMantenimientoViewModel mtto)
        {
            if (string.IsNullOrEmpty(mtto.responsable))
                throw new Exception("No has elegido un usuario");

            return new Mantenimiento
            {
                IdBien = (int)mtto.equipoAgregado[0].Id!,
                FechaRegistro = DateTime.Now,
                FechaMantenimiento = (DateTime)mtto.fechaMantenimiento!,
                Odei = mtto.Odei!,
                Responsable = mtto.responsable,
                Observacion = mtto.observacion,
                Garantia = mtto.garantia,
                Estado = mtto.estado,
                Operatividad = (bool)mtto.operatividad!,
                EquipoUso = (bool)mtto.equipoUso!,
                EquipoDadoBaja = (bool)mtto.equipoDadoBaja!
            };
        }
    }
}