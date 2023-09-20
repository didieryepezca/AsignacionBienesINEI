using AsignacionBienesINEI.Data.IUnitOfWork;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace AsignacionBienesINEI.Services
{
    public class GenericService : IGenericService
    {
        private readonly IUnitOfWorkSQLServer _unitOfWork;

        public GenericService(IUnitOfWorkSQLServer unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> InsertEntity<TResult>(TResult entity) where TResult : class
        {
            int result = 0;
            try 
            { 
                await _unitOfWork.GenericRepository.InsertEntity<TResult>(entity);
                result = await _unitOfWork.SaveChanges();
                return result;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Se ha producido un error al intentar guardar los cambios: {ex.Message}");
                return result;
            }            
        }

        public EntityEntry DeleteEntity<TResult>(TResult entity) where TResult : class
        {
            var result = _unitOfWork.GenericRepository.DeleteEntity<TResult>(entity);
            return result;
        }

        public Task<bool> EntityExistsByAllProperties<TResult>(TResult entity) where TResult : class
        {
            var result = _unitOfWork.GenericRepository.EntityExistsByAllProperties(entity);
            return result;
        }

        public Task<bool> EntityExistsByCustomConditions<TResult>(Expression<Func<TResult, bool>> predicate) where TResult : class
        {
            var result = _unitOfWork.GenericRepository.EntityExistsByCustomConditions(predicate);
            return result;
        }

        public async Task<IEnumerable<TResult>> GetAllEntitiesByConditions<TResult>(Expression<Func<TResult, bool>> predicate) where TResult : class
        {
            var result = await _unitOfWork.GenericRepository.GetAllEntitiesByConditions<TResult>(predicate);
            return result;
        }

        public Task<IEnumerable<TResult>> GetAllEntity<TResult>() where TResult : class
        {
            var result = _unitOfWork.GenericRepository.GetAllEntity<TResult>();
            return result;
        }

        public Task<TResult> GetEntity<TResult>(int id) where TResult : class
        {
            var result = _unitOfWork.GenericRepository.GetEntity<TResult>(id);
            return result;
        }

        public async Task<int> GetAndUpdateBien(Bien bien, string accion) 
        {
            int result = 0;
            try 
            {
                Bien bienDB = await _unitOfWork.GenericRepository.GetEntity<Bien>(bien.Id);                

                bienDB.Numero = bien.Numero;
                bienDB.Plaqueta = bien.Plaqueta;
                bienDB.Sbn = bien.Sbn;
                bienDB.Nombre = bien.Nombre;
                bienDB.Marca = bien.Marca;
                bienDB.Modelo = bien.Modelo;
                bienDB.Serie = bien.Serie;
                bienDB.Tipo = bien.Serie;
                bienDB.Dimensiones = bien.Dimensiones;
                bienDB.Color = bien.Color;
                bienDB.Estado = bien.Estado;
                bienDB.Operativo = bien.Operativo;
                bienDB.Observaciones = bien.Observaciones;
                bienDB.Proy = bien.Proy;

                if (accion == "A")
                {
                    _unitOfWork.GenericRepository.UpdateEntity<Bien>(bienDB);
                }
                else
                {
                    _unitOfWork.GenericRepository.DeleteEntity<Bien>(bienDB);
                }
                result = await _unitOfWork.SaveChanges();
                return result;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Se ha producido un error al intentar guardar los cambios: {ex.Message}");
                return result;
            }
        }
    }
}