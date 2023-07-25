using Confectionery.Data;
using Confectionery.Data.Entities;
using Confectionery.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//TODO: Make strongest password
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    //cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    //cfg.SignIn.RequireConfirmedEmail = true;
    cfg.User.RequireUniqueEmail = true;  //Condiciones
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    //cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //cfg.Lockout.MaxFailedAccessAttempts = 3;
    //cfg.Lockout.AllowedForNewUsers = true;

}).AddEntityFrameworkStores<DataContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/NotAuthorized";
	options.AccessDeniedPath = "/Account/NotAuthorized";
});


builder.Services.AddTransient<SeedDB>();
builder.Services.AddScoped<IUserHelper,UserHelper>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
var app = builder.Build();
seedData();

void seedData()
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory.CreateScope())
    {
        SeedDB? service = scope.ServiceProvider.GetService<SeedDB>();
        service.SeedAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
