using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using TechNews.Areas.BackEnd.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    // �]�w Session ���Įɶ��� 30 ����
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/BackEnd/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/BackEnd/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
});

builder.Services.AddDbContext<TechNewsDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Areas�ϰ�Route�t�m �]Controller�ݼW�[�ݩ� EX�G[Area("Admin")]�^
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}/");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
