using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// ***************************************************************************************
// * CUSTOM CODE
// * string connectionString = @"server=DESKTOP-MAE99H0; database=StudentDb; trusted_connection=true; TrustServerCertificate=true";
// * builder.Services.AddDbContext<StudentDbContext>(options => options.UseSqlServer(connectionString));
// * ABOVE TWO LINES ARE NOT NEEDED WHEN USING NAMED CONNECTION STRING
// * ONLY BELOW SERVICE REGISTRATION IS ENOUGH
//***************************************************************************************
builder.Services.AddDbContext<StudentDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
