using AromaBooks;
using AromaBooks.Areas.Admin.Services;
using AromaBooks.Data;
using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using AromaBooks.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
                .AddNToastNotifyToastr(new ToastrOptions()
                {
                    ProgressBar = true,
                    PositionClass = ToastPositions.TopRight,
                    TimeOut = 3000
                });
            


builder.Services.AddDbContext<AromaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalSqlServer")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
        options.Password = new PasswordOptions
        {
            RequireDigit = true,
            RequiredLength = 8,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireNonAlphanumeric = true,
        })
        .AddEntityFrameworkStores<AromaDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddTransient<ICategoryInterface , CategoryServices>();
builder.Services.AddTransient<IBookInterface,  BookService>();
builder.Services.AddTransient<IFileInterface, FileService>();




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
    name: "areas",
    pattern: "{area:exists}/{controller=Category}/{action=Index}/{id?}"
    );

app.UseNToastNotify();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.SeedRolesToDatabase().Wait();
app.Run();