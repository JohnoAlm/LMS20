using LMS20.Core.Entities;
using LMS20.Core.Repositories;
using LMS20.Core.Services;
using LMS20.Data.Data;
using LMS20.Web.Automapper;
using LMS20.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddScoped<IValidateDateService, ValidateDate>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// SeedData

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureDeleted();
    db.Database.Migrate();

    var config = services.GetRequiredService<IConfiguration>();
    var teacherPW = "abc123";
    var studentPW = "abc123";

    try
    {
        SeedData.InitAsync(db, services, teacherPW, studentPW).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Courses/Error");
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
    pattern: "{controller=Courses}/{action=Index}");
app.MapRazorPages();

app.Run();
