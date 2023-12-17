using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using RoadEventsProject.BLL.Services;
using RoadEventsProject.BLL.Services.Base;
using RoadEventsProject.DAL.DBContext;
using RoadEventsProject.DAL.Repositories;
using RoadEventsProject.DAL.Repositories.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RoadEventsContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddScoped<IRoadEventsRepository, RoadEventsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IViolationRepository, ViolationRepository>();
builder.Services.AddScoped<IViolationTypesConnectedsRepository, ViolationTypesConnectedsRepository>();
builder.Services.AddScoped<IPhotoVideoRepository, PhotoVideoRepository>();


builder.Services.AddScoped<IRoadEventsService, RoadEventsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IViolationService, ViolationService>();
builder.Services.AddScoped<IViolationTypesConnectedsService, ViolationTypesConnectedsService>();
builder.Services.AddScoped<IPhotoVideoService, PhotoVideoService>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

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
