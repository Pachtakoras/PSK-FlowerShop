using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;
using FlowerShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PSI_Food_waste.Data;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FlowerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<FlowerContext>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<IProductRepo, ProductRepo>();
builder.Services.AddTransient<ICategoryRepo, CategoryRepo>();


builder.Services.AddTransient<IOrderRepo, OrderRepo>();
builder.Services.AddTransient<IProductRepositoryDecorator>(provider =>
{
    var repository = provider.GetRequiredService<IProductRepo>();
    var cache = provider.GetRequiredService<IMemoryCache>();
    return new CashingProductRepository(repository, cache);
});


builder.Services.AddRazorPages();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FlowerContext>()
    .AddDefaultTokenProviders().AddDefaultUI();
void ConfigureContainer(ContainerBuilder builder)
{
    builder.RegisterType<ProductRepo>().As<IProductRepo>()
            .EnableInterfaceInterceptors()
            .InstancePerDependency();

    builder.RegisterType<OrderRepo>().As<IOrderRepo>()
            .EnableInterfaceInterceptors()
            .InstancePerDependency();

    builder.RegisterType<CategoryRepo>().As<ICategoryRepo>()
            .EnableInterfaceInterceptors()
            .InstancePerDependency();
}


builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.User.RequireUniqueEmail = true;
});


var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<FlowerContext>();
SeedData.Initialize(context);




app.MapControllerRoute(
    name: "products",
    pattern: "/products/{categorySlug?}",
    defaults: new { controller = "Products", action = "Index" });

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.MapRazorPages();



using(var scope = app.Services.CreateScope())
{
    var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Manager", "Member" };
    foreach (var role in roles)
    {
        if (!await rolesManager.RoleExistsAsync(role))
            await rolesManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string email = "admin@admin.com";
    string password = "Admin123admin#";
    if(await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            Email = email,
            Address = "admin",
            UserName = email,
            FirstName = "admin",
            LastName = "admin"
        };
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
    

}



app.Run();

void AddAuthorizationPolicies()
{
    builder.Services.AddAuthorization(options => {
        options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Administrator"));
    });
}