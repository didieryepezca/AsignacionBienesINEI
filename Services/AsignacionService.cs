using AsignacionBienesINEI.Data.IUnitOfWork;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;
using AsignacionBienesINEI.Services.IServices;

namespace AsignacionBienesINEI.Services
{
    public class AsignacionService : IAsignacionService
    {
        private readonly IUnitOfWorkSQLServer _unitOfWork;

        public AsignacionService(IUnitOfWorkSQLServer unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(int, int)> CreateAsignacionWithDetails(NuevaAsignacionViewModel nuevaAsignacionViewModel)
        {
            int result = 0;
            try
            {
                var asign = new Asignacion()
                {
                    IdUsuario = nuevaAsignacionViewModel.idUsuario,
                    IdEstado = 1, //SIN FICHA DE ASIGNACION,
                    EquipoFueraINEI = nuevaAsignacionViewModel.equipoFueraINEI,
                    EquipoLibre = nuevaAsignacionViewModel.equipoLibre,
                    Observacion = nuevaAsignacionViewModel.observacion!,
                    FechaAsignacion = DateTime.Now,
                };

                var detList = new List<AsignacionDetalle>();
                foreach (var equipo in nuevaAsignacionViewModel.asignacionesAgregadas)
                {
                    var newDet = new AsignacionDetalle
                    {
                        Asignacion = asign,
                        IdBien = Convert.ToInt16(equipo.Id),
                        FechaIngreso = DateTime.Now,
                    };
                    detList.Add(newDet);
                }

                await _unitOfWork.GenericRepository.InsertEntity(asign);
                await _unitOfWork.AsignacionDetalleRepository.InsertAsignacionDetalles(detList);
                result = await _unitOfWork.SaveChanges();
                return (result,asign.Id);
            }
            catch (Exception ex) 
            {   
                Console.WriteLine($"Se ha producido un error al intentar guardar los cambios: {ex.Message}");                
                return (result, 0);
            }
        }

        public async Task<int> UpdateEquipmentStatus(NuevaAsignacionViewModel nuevaAsignacionViewModel) 
        {
            int result = 0;
            try 
            {
                ////Actualizar estado de los equipós que ya se asignaron...
                foreach (var equipoInDet in nuevaAsignacionViewModel.asignacionesAgregadas)
                {
                    var equipo = await _unitOfWork.GenericRepository.GetEntity<Bien>(equipoInDet.Id);
                    equipo.Asignado = true;
                    _unitOfWork.GenericRepository.UpdateEntity<Bien>(equipo);                    
                    result++;
                }
                result = await _unitOfWork.SaveChanges();
                return (int)result;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Se ha producido un error al intentar guardar los cambios: {ex.Message}");
                return result;
            }
        }

        public async Task<int> UploadSignedAsignationPDF(IFormFile file,int idAsignacion) 
        {
            int result = 0;
            try 
            {
                byte[] pdfBytesArray;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    pdfBytesArray = memoryStream.ToArray();
                }

                var newFileRegistry = new AsignacionPDF()
                {
                    IdAsignacion = idAsignacion,
                    PdfData = pdfBytesArray,
                    FechaCarga = DateTime.Now
                };

                await _unitOfWork.GenericRepository.InsertEntity<AsignacionPDF>(newFileRegistry);
                Asignacion asign = await _unitOfWork.GenericRepository.GetEntity<Asignacion>(idAsignacion);
                asign.IdEstado = 2;
                _unitOfWork.GenericRepository.UpdateEntity<Asignacion>(asign);

                result = await _unitOfWork.SaveChanges();
                return result;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Se ha producido un error al intentar guardar los cambios: {ex.Message}");
                return result;
            }
        }

        public async Task<int> UpdateAsignationAndReleaseEquipments(int idAsignacion, int idBien) 
        {
            int result = 0;
            try 
            {
                var asignacion = await _unitOfWork.GenericRepository.GetEntity<Asignacion>(idAsignacion);
                asignacion.IdEstado = 3;
                _unitOfWork.GenericRepository.UpdateEntity<Asignacion>(asignacion);

                var equipo = await _unitOfWork.GenericRepository.GetEntity<Bien>(idBien);
                equipo.Asignado = false;
                _unitOfWork.GenericRepository.UpdateEntity<Bien>(equipo);

                result = await _unitOfWork.SaveChanges();
                return result;
            } 
            catch(Exception ex) 
            {
                Console.WriteLine($"Se ha producido un error al intentar guardar los cambios: {ex.Message}");
                return result;
            }
        }

        public async Task<AsignacionPDF> FindAsignacionFirmadaPDF(int idAsignacion)
        {
            AsignacionPDF asignacionPDF = await _unitOfWork.AsignacionRepository.FindAsignacionFirmadaPDF(idAsignacion);
            return asignacionPDF;            
        }

        public async Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesByDateAndStatus(int status, DateTime date) 
        {
            IEnumerable<AsignacionDetalle> asignaciones = 
                await _unitOfWork.AsignacionRepository.GetAllAsignacionesByEstadoAndFecha(status, date);

            return asignaciones;                
        }

        public async Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesPorUsuario(string userId)
        {
            IEnumerable<AsignacionDetalle> details = await _unitOfWork.AsignacionRepository.GetAllAsignacionesPorUsuario(userId);
            return details;
        }

        public async Task<IEnumerable<AsignacionDetalle>> GetAsignacionPorId(int idAsignacion)
        {
            IEnumerable<AsignacionDetalle> asignaciones = await _unitOfWork.AsignacionRepository.GetAsignacionPorId(idAsignacion);
            return asignaciones;
        }

        public async Task<IEnumerable<Bien>> SearchBienPorControlPatrimonial(string codigoPatrimonial, string tipo)
        {
            IEnumerable<Bien> query = await _unitOfWork.AsignacionRepository.SearchBienPorControlPatrimonial(codigoPatrimonial, tipo);
            return query;
        }
    }
}