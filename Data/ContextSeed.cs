using AsignacionBienesINEI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AsignacionBienesINEI.Data
{
    public class ContextSeed
    {
        public static async Task SeedAllUsersFromPersonelDataAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            if (userManager.Users.ToList().Count == 1)
            {
                using (var db = dbContext)
                {
                    var personel = await db.Personel.ToListAsync();
                    foreach (Personel person in personel)
                    {
                        //Seed New Users
                        var newUser = new ApplicationUser
                        {
                            Odei = "CAJAMARCA",
                            Nombres = (string.IsNullOrEmpty(person.ApellidosNombres)) ? string.Empty : person.ApellidosNombres.Trim(),
                            ApellidoPaterno = string.Empty,
                            ApellidoMaterno = string.Empty,
                            DNI = (string.IsNullOrEmpty(person.Dni)) ? string.Empty : person.Dni.Trim(),
                            CondicionLaboral = (string.IsNullOrEmpty(person.CondicionLaboral)) ? string.Empty : person.CondicionLaboral.Trim(),
                            FechaNacimiento = (string.IsNullOrEmpty(person.FechaNacimiento)) ? string.Empty : person.FechaNacimiento.Trim(),
                            ProfesionEstudio = (string.IsNullOrEmpty(person.Profesion)) ? string.Empty : person.Profesion.Trim(),
                            GradoAcademico = (string.IsNullOrEmpty(person.GradoAcademico)) ? string.Empty : person.GradoAcademico.Trim(),
                            TelefonoCelular = (string.IsNullOrEmpty(person.Celular)) ? string.Empty : person.Celular.Trim(),
                            FechaRegistro = DateTime.Now,
                            UserName = $"{person.Dni}@asignacionbienes.com",
                            Email = $"{person.Dni}@asignacionbienes.com",                            
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true
                        };

                        try 
                        {
                            var result = await userManager.CreateAsync(newUser, "123456"); //Crear Usuario.
                        }
                        catch (Exception ex)
                        {                        
                            Console.WriteLine(ex.Message);
                        }                        
                    }
                }
            }
        }
    }}