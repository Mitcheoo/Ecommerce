using Ecommerce.Data;
using Ecommerce.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Thêm dòng này để bật session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian hết hạn session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<DatabaseEcommerceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HShop"));
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile)); //automapper
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{     options.LoginPath = "/KhachHang/DangNhap";
     
    options.AccessDeniedPath = "/AccessDenied";///moi dang nhap chua co quyen thi se chuyen den day
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ Kích hoạt session middleware
app.UseSession();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
