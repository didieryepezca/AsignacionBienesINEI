using AsignacionBienesINEI.BusinessLogic.ILogic;
using AsignacionBienesINEI.BusinessLogic.Logic;
using AsignacionBienesINEI.Data;
using AsignacionBienesINEI.Data.IRepository;
using AsignacionBienesINEI.Data.IUnitOfWork;
using AsignacionBienesINEI.Data.Repository;
using AsignacionBienesINEI.Data.UnitOfWork;
using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Services;
using AsignacionBienesINEI.Services.IServices;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SQLServerConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); //para enviar el Token por default de confirmaci�n de email.

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorRuntimeCompilation(); //Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation

builder.Services.Configure<FormOptions>(options => { //Permitir Archivos subir archivos grandes a través de los formularios.
    options.MultipartBodyLengthLimit = 60000000;
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//Repositorys
builder.Services.AddTransient<IGenericEntityRepository<object>, GenericEntityRepository>();
builder.Services.AddTransient<IAsignacionRepository, AsignacionRepository>();
builder.Services.AddTransient<IAsignacionDetalleRepository, AsignacionDetalleRepository>();    
//Logics
builder.Services.AddTransient<IAsignacionBL, AsignacionBL>();
builder.Services.AddTransient<IMantenimientoBL, MantenimientoBL>();
//Services
builder.Services.AddTransient<IUnitOfWorkSQLServer, UnitOfWorkSQLServer>();
builder.Services.AddTransient<IAsignacionService, AsignacionService>();
builder.Services.AddTransient<IGenericService, GenericService>();

var app = builder.Build();

//----------> Setear Usuario Admin y SetRoles.
using var scope = app.Services.CreateScope();

//var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//await ContextSeed.SeedAllUsersFromPersonelDataAsync(userManager,dbContext);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();