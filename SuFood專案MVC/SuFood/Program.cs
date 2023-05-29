using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuFood.Data;
using SuFood.Models;
using SuFood.Services;
using Microsoft.AspNetCore.Http;

namespace SuFood
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //SuFood Connection
            var SuFoodConnection = builder.Configuration.GetConnectionString("SuFood") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<SuFoodDBContext>(options =>
                options.UseSqlServer(SuFoodConnection));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            

            // 啟用Session
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".CustomerswebSite.Session";//自session名稱
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(5);//設定網頁5分鐘逾時
                options.Cookie.HttpOnly = true;//設定Request只能透過HTTP傳送
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });


             builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {                    
                   options.AccessDeniedPath = "/User/Login"; //登入失敗路徑
                   options.LogoutPath = "/Home/Index";  //登出路徑
                   options.ExpireTimeSpan = TimeSpan.FromDays(30); //Cookie 預期時間                   
                   options.Cookie.IsEssential = true;//設定session Id，可透過你設定的id找到哪個是你的               
                   options.Cookie.HttpOnly = true;//設定Request只能透過HTTP傳送
                   options.Cookie.SecurePolicy = CookieSecurePolicy.Always;//設定傳送時一定要透過加密方式傳送
               });
            

            builder.Services.AddTransient<EncryptService>();


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
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}