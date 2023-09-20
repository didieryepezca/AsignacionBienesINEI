using AsignacionBienesINEI.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AsignacionBienesINEI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext()
        {
            Database.SetCommandTimeout(0); //X minutos de ejecucion
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public virtual DbSet<ApplicationUser>  ApplicationUser { get; set; }

        public virtual DbSet<Personel> Personel { get; set; }
        
        public virtual DbSet<Bien> Bien { get; set; }

        public virtual DbSet<Asignacion> Asignacion { get; set; }

        public virtual DbSet<AsignacionEstado> AsignacionEstado { get; set; }

        public virtual DbSet<AsignacionDetalle> AsignacionDetalle { get; set; }

        public virtual DbSet<AsignacionPDF> AsignacionPDF { get; set; }

        public virtual DbSet<Mantenimiento> Mantenimiento { get; set; }
    }
}