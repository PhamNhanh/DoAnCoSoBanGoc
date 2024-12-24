using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;
using WEBTimViec.Services.VnPay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{

})
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddControllersWithViews();
// Add scoped services for repositories
 
builder.Services.AddScoped<IKinhNghiem, EFKinhNghiemRepository>();
builder.Services.AddScoped<IChuyenNganh, EFChuyenNganhRepository>();
builder.Services.AddScoped<IBaiTuyenDung, EFBaiTuyenDungRepository>();
builder.Services.AddScoped<IUngTuyen, EFUngTuyenRepository>();
builder.Services.AddScoped<IThanhPho, EFThanhPhoRepository>();
builder.Services.AddScoped<IViTriCongViec, EFViTriCongViecRepository>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IKyNangMem, EFKyNangMemRepository>();
builder.Services.AddScoped<IHocVan, EFHocVanRepository>();
builder.Services.AddScoped<ITruongDaiHoc, EFTruongDaiHocRepository>();
builder.Services.AddScoped<ISaveJob, EFSaveJob>();
builder.Services.AddScoped<ILoaiTaiKhoan, EFLoaiTaiKhoan>();
//Connect VNPay API
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.AddRazorPages();

var app = builder.Build();
 

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
    name: "Admin",
    pattern: "{area:exists}/{controller=AD}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "NhaTuyenDung",
    pattern: "{area:exists}/{controller=NTD}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "UngVien",
    pattern: "{area:exists}/{controller=UV}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=IndexAll}/{id?}");

app.MapRazorPages();

app.Run();
