
using IdentityMail.Web.Context;
using IdentityMail.Web.CustomValidation;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// SQL SERVER BAĞLANTISI
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration
        .GetConnectionString("DefaultConnection");

    options.UseSqlServer(connectionString);
});


// IDENTITY AYARLARI
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddErrorDescriber<CustomErrorDescriber>();


// SECURITY STAMP AYARLARI
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});


// COOKIE AYARLARI
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.Cookie.Name = "IdentityMailCookie";
});


builder.Services.AddControllersWithViews();

var app = builder.Build();


// HTTP PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// ADMIN AREA ROUTING
app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


// NORMAL MVC ROUTING
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ROL OLUŞTURMA
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<AppRole>>();


    // ADMIN ROLÜ OLUŞTURMA
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var result = await roleManager.CreateAsync(
            new AppRole { Name = "Admin" });

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ",
                    result.Errors.Select(x => x.Description)));
        }
    }


    // USER ROLÜ OLUŞTURMA
    if (!await roleManager.RoleExistsAsync("User"))
    {
        var result = await roleManager.CreateAsync(
            new AppRole { Name = "User" });

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ",
                    result.Errors.Select(x => x.Description)));
        }
    }


    // GELİŞTİRME ORTAMINDA ADMIN YETKİSİ VER
    if (app.Environment.IsDevelopment())
    {
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<AppUser>>();

        var adminEmail = "Mehmetshn@gmail.com";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            throw new InvalidOperationException(
                "Admin yapılacak kullanıcı bulunamadı.");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var result = await userManager.AddToRoleAsync(
                adminUser, "Admin");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        result.Errors.Select(x => x.Description)));
            }
        }
    }
}

app.Run();
