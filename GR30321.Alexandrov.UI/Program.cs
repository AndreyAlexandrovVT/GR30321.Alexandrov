//using GR30321.API.Data;
using GR30321.Alexandrov.UI.Data;
using GR30321.Alexandrov.UI.Middleware;
using GR30321.Alexandrov.UI.Models;
using GR30321.Alexandrov.UI.Services.BookService;
using GR30321.Alexandrov.UI.Services.CategoryService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SqLiteConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(connectionString));
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; //������������� ����������� �����
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//����_3
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p =>
    p.RequireClaim(ClaimTypes.Role, "admin"));
});
builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>(); //


builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<ICategoryService, MemoryCetegoryService>();
//builder.Services.AddScoped<IBookService, MemoryBookService>();


builder.Services.AddHttpClient<IBookService, ApiBookService>(opt=> opt.BaseAddress = new Uri("https://localhost:7002/api/books/"));
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt=> opt.BaseAddress = new Uri("https://localhost:7002/api/categories/"));


builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

Log.Logger = new LoggerConfiguration()
 .WriteTo.Console()
 .WriteTo.File("logs/log.txt", rollingInterval:
RollingInterval.Day)
 .CreateLogger();


//для лабораторной 7
//builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlite(""));


var app = builder.Build();

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
app.UseSession();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<FileLogger>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

await GR30321.Alexandrov.UI.Data.DbInit.SetupIdentityAdmin(app);



app.Run();

