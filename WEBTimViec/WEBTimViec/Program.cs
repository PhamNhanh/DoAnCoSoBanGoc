using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Add scoped services for repositories
builder.Services.AddScoped<ITruongDaiHoc, EFTruongDaiHocRepository>();
builder.Services.AddScoped<IKinhNghiem, EFKinhNghiemRepository>();
builder.Services.AddScoped<IChuyenNganh, EFChuyenNganhRepository>();
builder.Services.AddScoped<IBaiTuyenDung, EFBaiTuyenDungRepository>();
builder.Services.AddScoped<IUngTuyen, EFUngTuyenRepository>();
builder.Services.AddScoped<IHocVan, EFHocVanRepository>();
builder.Services.AddRazorPages();

var app = builder.Build();


builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure the app uses authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
